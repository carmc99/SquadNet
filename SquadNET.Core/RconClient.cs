using SquadNET.Core;
using SquadNET.Core.Squad;
using SquadNET.Core.Squad.Entities;
using System;
using System.Net;
using System.Net.Sockets;

namespace SquadNET.Rcon
{
    /// <summary>
    /// Represents an RCON (Remote Console) client capable of connecting,
    /// authenticating, and sending commands to a remote server over TCP.
    /// </summary>
    public class RconClient : IDisposable
    {
        private readonly IPEndPoint EndPoint;
        private readonly string Password;

        private Socket Socket;
        private NetworkStream Stream;
        private bool IsConnected;
        private int PacketIdCounter = 3;
        private readonly CancellationTokenSource CancellationTokenSource;

        /// <summary>
        /// Occurs when the connection is successfully established.
        /// </summary>
        public event Action OnConnected;

        /// <summary>
        /// Occurs when a valid <see cref="Packet"/> is received from the server.
        /// </summary>
        public event Action<PacketInfo> OnPacketReceived;

        /// <summary>
        /// Occurs when an unhandled exception is thrown during communication.
        /// </summary>
        public event Action<Exception> OnExceptionThrown;

        /// <summary>
        /// Occurs when raw bytes are received from the server.
        /// </summary>
        public event Action<byte[]> OnBytesReceived;

        /// <summary>
        /// Initializes a new instance of the <see cref="RconClient"/> class.
        /// </summary>
        /// <param name="endPoint">The server endpoint (IP + port) for the RCON connection.</param>
        /// <param name="password">The password used for RCON authentication.</param>
        /// <exception cref="ArgumentNullException">Thrown if endPoint or password is null.</exception>
        public RconClient(IPEndPoint endPoint, string password)
        {
            EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Establishes a connection to the RCON server and performs authentication.
        /// If already connected, no action is taken.
        /// </summary>
        /// <exception cref="Exception">Thrown if the connection or authentication fails.</exception>
        public void Connect()
        {
            if (IsConnected) return;

            try
            {
                Socket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Socket.Connect(EndPoint);

                Stream = new NetworkStream(Socket);
                Authenticate();

                IsConnected = true;
                OnConnected?.Invoke();

                Task.Run(() => ListenForResponses(CancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to connect to RCON server: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Closes the RCON connection and releases the underlying resources.
        /// If already disconnected, no action is taken.
        /// </summary>
        public void Disconnect()
        {
            if (!IsConnected) return;

            CancellationTokenSource.Cancel();

            if (Stream != null)
            {
                Stream.Dispose();
                Stream = null;
            }

            if (Socket != null)
            {
                Socket.Close();
                Socket = null;
            }

            IsConnected = false;
        }

        /// <summary>
        /// Authenticates the client using the configured password.
        /// If authentication fails, an exception is thrown.
        /// </summary>
        /// <exception cref="Exception">Thrown if authentication fails (invalid password).</exception>
        private void Authenticate()
        {
            PacketInfo authPacket = new PacketInfo(GetNextPacketId(), PacketType.ServerDataAuth, Password);
            SendPacket(authPacket);

            PacketInfo response = ReceivePacket();
            if (response.Id == -1)
            {
                throw new Exception("Authentication failed: invalid RCON password.");
            }
        }

        /// <summary>
        /// Sends an RCON command to the server and returns the raw byte response.
        /// Execution is performed synchronously within a Task.Run block.
        /// </summary>
        /// <param name="command">The command string to send (e.g., "ListPlayers").</param>
        /// <param name="cancellationToken">Cancellation token for this command request.</param>
        /// <returns>The server's response as a byte array.</returns>
        /// <exception cref="Exception">Thrown if reading the response fails or the connection is closed prematurely.</exception>
        public async Task<byte[]> WriteCommandAsync(string command, CancellationToken cancellationToken)
        {
            int packetId = GetNextPacketId();
            PacketInfo commandPacket = new PacketInfo(packetId, PacketType.ServerDataExecCommand, command);
            SendPacket(commandPacket);

            PacketInfo response = await Task.Run(() => ReceivePacket(), cancellationToken);
            return response.Body;
        }

        /// <summary>
        /// Listens for packets from the server in a continuous loop, invoking
        /// events as packets are received. Exits when cancellation is requested.
        /// </summary>
        /// <param name="cancellationToken">Token that signals when the loop should stop.</param>
        private void ListenForResponses(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    PacketInfo packet = ReceivePacket();
                    OnPacketReceived?.Invoke(packet);
                }
            }
            catch (Exception ex)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    OnExceptionThrown?.Invoke(ex);
                }
            }
        }

        /// <summary>
        /// Reads a single <see cref="Packet"/> from the <see cref="Stream"/>.
        /// The method ensures it reads exactly the required number of bytes
        /// both for the packet size header and the actual packet content.
        /// </summary>
        /// <returns>The decoded <see cref="Packet"/>.</returns>
        /// <exception cref="Exception">Thrown if the connection closes or the
        /// required bytes cannot be read fully.</exception>
        private PacketInfo ReceivePacket()
        {
            // 1) Read the 4-byte size header.
            byte[] sizeBuffer = new byte[4];
            ReadExact(sizeBuffer, 0, sizeBuffer.Length);
            int packetSize = BitConverter.ToInt32(sizeBuffer, 0);

            // 2) Read the packet content of length 'packetSize'.
            byte[] packetBuffer = new byte[packetSize];
            ReadExact(packetBuffer, 0, packetSize);

            PacketInfo packet = PacketInfo.Parse(packetBuffer);
            OnBytesReceived?.Invoke(packetBuffer);

            return packet;
        }

        /// <summary>
        /// Sends the specified <see cref="Packet"/> to the server via the <see cref="Stream"/>.
        /// </summary>
        /// <param name="packet">The packet to send.</param>
        private void SendPacket(PacketInfo packet)
        {
            byte[] packetBytes = packet.ToArray();
            Stream.Write(packetBytes, 0, packetBytes.Length);
        }

        /// <summary>
        /// Reads the exact number of bytes from the underlying <see cref="Stream"/>
        /// into the provided buffer, blocking until all bytes have been received
        /// or the connection is closed.
        /// </summary>
        /// <param name="buffer">Destination buffer.</param>
        /// <param name="offset">Offset in the buffer to begin writing data.</param>
        /// <param name="count">Number of bytes to read.</param>
        /// <exception cref="Exception">Thrown if the required number of bytes
        /// cannot be read (indicating a closed or unreliable connection).</exception>
        private void ReadExact(byte[] buffer, int offset, int count)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int bytesRead = Stream.Read(buffer, offset + totalRead, count - totalRead);
                if (bytesRead <= 0)
                {
                    throw new Exception("Connection closed or unable to read the requested number of bytes.");
                }
                totalRead += bytesRead;
            }
        }

        /// <summary>
        /// Obtains the next available packet ID in a thread-safe manner
        /// by incrementing <see cref="PacketIdCounter"/>.
        /// </summary>
        private int GetNextPacketId()
        {
            return Interlocked.Increment(ref PacketIdCounter);
        }

        /// <summary>
        /// Disconnects from the server and disposes underlying resources.
        /// </summary>
        public void Dispose()
        {
            Disconnect();
            CancellationTokenSource.Dispose();
        }
    }
}
