// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SquadNET.Rcon
{
    public class RconClient : IDisposable
    {
        private static readonly TimeSpan ReconnectTimeout = TimeSpan.FromSeconds(2);
        private readonly IPEndPoint EndPoint;
        private readonly ConcurrentQueue<PacketInfo[]> PackageWriteQueue = new();
        private readonly string Password;
        private readonly ConcurrentDictionary<int, RconClientCommandResult> PendingCommandResults = new();
        private int PacketInfoIdCounter = 3;
        private CancellationTokenSource ThreadCancellationTokenSource;
        private Thread WorkerThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="RconClient"/> class.
        /// </summary>
        /// <param name="endPoint">The endpoint to connect to.</param>
        /// <param name="password">The RCON password.</param>
        public RconClient(IPEndPoint endPoint, string password)
        {
            EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public event Action<byte[]>? BytesReceived;

        public event Action? Connected;

        public event Action<Exception>? ExceptionThrown;

        public event Action<PacketInfo>? PacketReceived;

        /// <summary>
        /// Gets a value indicating whether the client is started.
        /// </summary>
        public bool IsStarted => WorkerThread != null;

        /// <summary>
        /// Disposes the client and stops the connection.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// Starts the RCON client and establishes a connection.
        /// </summary>
        public void Start()
        {
            if (IsStarted)
            {
                return;
            }

            ThreadCancellationTokenSource = new CancellationTokenSource();
            WorkerThread = new Thread(ThreadHandler)
            {
                IsBackground = true
            };
            WorkerThread.Start();
        }

        /// <summary>
        /// Stops the RCON client and disconnects.
        /// </summary>
        public void Stop()
        {
            foreach (KeyValuePair<int, RconClientCommandResult> result in PendingCommandResults)
            {
                result.Value.Cancel();
            }

            PendingCommandResults.Clear();
            PackageWriteQueue.Clear();
            PacketInfoIdCounter = 3;

            ThreadCancellationTokenSource?.Cancel();
            WorkerThread?.Join();
            ThreadCancellationTokenSource?.Dispose();

            ThreadCancellationTokenSource = null;
            WorkerThread = null;
        }

        /// <summary>
        /// Writes a command to the RCON server and waits for the response.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response from the server.</returns>
        public async Task<string> WriteCommandAsync(string command, CancellationToken cancellationToken)
        {
            int packetInfoId = GetNextPacketInfoId();
            PacketInfo commandPacketInfo = new(
                packetInfoId,
                RconPacketType.ServerDataExecCommand,
                command,
                false,
                Encoding.UTF8
            );

            PacketInfo emptyPacketInfo = new(
                packetInfoId,
                RconPacketType.ServerDataExecCommand,
                []
            );

            RconClientCommandResult commandResult = new([commandPacketInfo, emptyPacketInfo]);

            PendingCommandResults[packetInfoId] = commandResult;
            WritePacketInfos(commandPacketInfo, emptyPacketInfo);

            IReadOnlyList<PacketInfo> packetInfos = await commandResult.Result;
            PacketInfo[] packetInfosWithoutEnd = packetInfos.ToArray()[..^1];

            return Encoding.UTF8.GetString(packetInfosWithoutEnd.SelectMany(x => x.Body).ToArray());
        }

        /// <summary>
        /// Generates the next packet ID in a thread-safe manner.
        /// </summary>
        /// <returns>The next packet ID.</returns>
        protected int GetNextPacketInfoId()
        {
            return Interlocked.Increment(ref PacketInfoIdCounter);
        }

        /// <summary>
        /// Invokes the BytesReceived event with the received bytes.
        /// </summary>
        /// <param name="bytes">The bytes received from the server.</param>
        protected virtual void OnBytesReceived(byte[] bytes)
        {
            BytesReceived?.Invoke(bytes);
        }

        /// <summary>
        /// Invokes the Connected event when a connection is established.
        /// </summary>
        protected virtual void OnConnected()
        {
            Connected?.Invoke();
        }

        /// <summary>
        /// Invokes the ExceptionThrown event when an exception occurs.
        /// </summary>
        /// <param name="exception">The exception that was thrown.</param>
        protected virtual void OnExceptionThrown(Exception exception)
        {
            ExceptionThrown?.Invoke(exception);
        }

        /// <summary>
        /// Invokes the PacketReceived event when a packet is received.
        /// </summary>
        /// <param name="packetInfo">The packet received from the server.</param>
        protected virtual void OnPacketReceived(PacketInfo packetInfo)
        {
            PacketReceived?.Invoke(packetInfo);
        }

        /// <summary>
        /// Processes a received packet and handles it accordingly.
        /// </summary>
        /// <param name="packetInfo">The packet to process.</param>
        protected virtual void ProcessPacketInfo(PacketInfo packetInfo)
        {
            if (packetInfo is { Id: 3, Type: RconPacketType.ServerDataResponseValue })
            {
                return;
            }

            try
            {
                OnPacketReceived(packetInfo);
            }
            catch (Exception e)
            {
                OnExceptionThrown(e);
            }

            if (!PendingCommandResults.TryGetValue(packetInfo.Id, out RconClientCommandResult command))
            {
                return;
            }

            command.AddPacketInfo(packetInfo);

            if (packetInfo is not
                {
                    Type: RconPacketType.ServerDataResponseValue,
                    Body: { Length: 0 }
                })
            {
                return;
            }

            command.Complete();
            PendingCommandResults.TryRemove(packetInfo.Id, out _);
        }

        /// <summary>
        /// Enqueues packets to be sent to the server.
        /// </summary>
        /// <param name="packetInfos">The packets to send.</param>
        protected void WritePacketInfos(params PacketInfo[] packetInfos)
        {
            PackageWriteQueue.Enqueue(packetInfos);
        }

        /// <summary>
        /// Receives a packet from the server.
        /// </summary>
        /// <param name="socket">The socket to receive data from.</param>
        /// <returns>The received packet.</returns>
        private static PacketInfo ReceivePacketInfo(Socket socket)
        {
            byte[] sizeBuffer = new byte[4];
            int bytesRead = socket.Receive(sizeBuffer);
            if (bytesRead != 4)
            {
                throw new Exception("Invalid bytes read for packet size.");
            }

            int packetInfoSize = PacketInfo.ParseSize(sizeBuffer);
            byte[] packetInfoBuffer = new byte[packetInfoSize];
            bytesRead = socket.Receive(packetInfoBuffer);
            if (bytesRead != packetInfoSize)
            {
                throw new Exception("Invalid bytes read for packet content.");
            }

            return PacketInfo.Parse(packetInfoBuffer);
        }

        /// <summary>
        /// Sends a single packet to the server.
        /// </summary>
        /// <param name="socket">The socket to send data to.</param>
        /// <param name="packetInfo">The packet to send.</param>
        private static void Send(Socket socket, PacketInfo packetInfo)
        {
            byte[] packetInfoBytes = packetInfo.ToArray();

            int bytesSent = socket.Send(packetInfoBytes);
            if (bytesSent != packetInfoBytes.Length)
            {
                throw new Exception("Failed to send the entire packet.");
            }
        }

        /// <summary>
        /// Sends multiple packets to the server in a single operation.
        /// </summary>
        /// <param name="socket">The socket to send data to.</param>
        /// <param name="packetInfos">The packets to send.</param>
        private static void Send(Socket socket, params PacketInfo[] packetInfos)
        {
            byte[] packetInfoBytes = packetInfos
                .SelectMany(x => x.ToArray())
                .ToArray();

            int bytesSent = socket.Send(packetInfoBytes);
            if (bytesSent != packetInfoBytes.Length)
            {
                throw new Exception("Failed to send all packets.");
            }
        }

        /// <summary>
        /// Shifts bytes left in a buffer to remove processed data.
        /// </summary>
        /// <param name="bytes">The buffer to shift.</param>
        /// <param name="shiftLength">The number of bytes to shift.</param>
        private static void ShiftBytesLeft(byte[] bytes, int shiftLength)
        {
            if (shiftLength >= bytes.Length)
            {
                throw new Exception("Invalid shift length.");
            }

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = i + shiftLength >= bytes.Length ? (byte)0 : bytes[i + shiftLength];
            }
        }

        /// <summary>
        /// Authenticates the client with the server using the provided password.
        /// </summary>
        /// <param name="socket">The socket to authenticate with.</param>
        private void Authenticate(Socket socket)
        {
            int authPacketInfoId = GetNextPacketInfoId();
            PacketInfo requestPacketInfo = new PacketInfo(
                authPacketInfoId,
                RconPacketType.ServerDataAuth,
                Password
            );
            Send(socket, requestPacketInfo);

            // Receive the first packet, which is an acknowledgment but not needed.
            ReceivePacketInfo(socket);
            PacketInfo packetInfo = ReceivePacketInfo(socket);

            if (packetInfo.Id == -1)
            {
                throw new Exception("Authentication failed: Invalid password.");
            }
        }

        /// <summary>
        /// Handles the connection to the server, including reading and writing packets.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token to stop the connection.</param>
        private void HandleConnection(CancellationToken cancellationToken)
        {
            using Socket socket = new(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, false);
            socket.LingerState = new LingerOption(false, 0);

            try
            {
                socket.Connect(EndPoint);
                Authenticate(socket);
                OnConnected();

                byte[] buffer = new byte[4096 + 7]; // Maximum packet size + 7 bytes for broken packets
                int actualBufferLength = 0;

                while (!cancellationToken.IsCancellationRequested)
                {
                    int dataAvailable = socket.Available;
                    if (dataAvailable > 0)
                    {
                        int dataToRead = Math.Min(buffer.Length - actualBufferLength, dataAvailable);

                        int bytesRead = socket.Receive(buffer, actualBufferLength, dataToRead, SocketFlags.None);
                        if (bytesRead != dataToRead)
                        {
                            throw new Exception("Failed to read the expected number of bytes.");
                        }

                        OnBytesReceived(buffer[actualBufferLength..(actualBufferLength + dataToRead)]);

                        actualBufferLength += bytesRead;
                    }

                    if (actualBufferLength > 0)
                    {
                        int packetInfoSize = PacketInfo.ParseSize(buffer[..4]);

                        if (packetInfoSize <= actualBufferLength)
                        {
                            ShiftBytesLeft(buffer, 4);
                            actualBufferLength -= 4;

                            PacketInfo packetInfo;

                            // Check for broken packets:
                            // The Squad server sends an invalid packet when appending an empty exec command packet.
                            // This packet is filtered out as it is not needed and does not conform to the Source RCON protocol.
                            if (packetInfoSize == 10)
                            {
                                byte[] maybeBrokenBuffer = buffer[..17];
                                packetInfo = PacketInfo.Parse(maybeBrokenBuffer);
                                if (packetInfo.IsBroken)
                                {
                                    ShiftBytesLeft(buffer, 17);
                                    actualBufferLength -= 17;
                                }
                                else
                                {
                                    packetInfo = PacketInfo.Parse(buffer[..10]);
                                    ShiftBytesLeft(buffer, 10);
                                    actualBufferLength -= 10;

                                    ProcessPacketInfo(packetInfo);
                                }
                            }
                            else
                            {
                                packetInfo = PacketInfo.Parse(buffer[..packetInfoSize]);
                                ShiftBytesLeft(buffer, packetInfoSize);
                                actualBufferLength -= packetInfoSize;

                                ProcessPacketInfo(packetInfo);
                            }
                        }
                    }

                    while (PackageWriteQueue.TryDequeue(out var packetInfoGroup))
                    {
                        Send(socket, packetInfoGroup);
                    }

                    Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                OnExceptionThrown(e);
            }
            finally
            {
                // Requeue all pending command results for retry on reconnection.
                PackageWriteQueue.Clear();
                PacketInfoIdCounter = 3;

                foreach ((int _, RconClientCommandResult result) in PendingCommandResults)
                {
                    result.ClearPacketInfos();
                    PackageWriteQueue.Enqueue(result.RequestPacketInfos);
                }

                socket.Disconnect(false);
                socket.Close();
                Thread.Sleep(ReconnectTimeout);
            }
        }

        /// <summary>
        /// Handles the worker thread that manages the connection to the server.
        /// </summary>
        /// <param name="_">Unused parameter.</param>
        private void ThreadHandler(object _)
        {
            CancellationTokenSource cancellationTokenSource = ThreadCancellationTokenSource;
            if (cancellationTokenSource == null)
            {
                throw new Exception("No cancellation token source provided for the thread.");
            }

            while (!cancellationTokenSource.IsCancellationRequested)
            {
                HandleConnection(cancellationTokenSource.Token);
            }
        }
    }
}