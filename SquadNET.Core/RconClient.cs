// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SquadNET.Rcon
{
    public class RconClient : IDisposable
    {
        private readonly CancellationTokenSource CancellationTokenSource;
        private readonly IPEndPoint EndPoint;
        private readonly HashSet<ushort> FinalizedResponses = new();
        private readonly List<byte> IncomingBuffer = [];
        private readonly string Password;
        private readonly Dictionary<ushort, List<string>> PendingPartialResponses = [];
        private readonly Dictionary<ushort, TaskCompletionSource<string>> PendingResponses = [];
        private bool IsConnected;
        private int PacketIdCounter = 3;
        private Socket Socket;
        private NetworkStream Stream;

        public RconClient(IPEndPoint endPoint, string password)
        {
            EndPoint = endPoint ?? throw new ArgumentNullException(nameof(endPoint));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            CancellationTokenSource = new CancellationTokenSource();
        }

        public event Action<byte[]> OnBytesReceived;

        public event Action OnConnected;

        public event Action<Exception> OnExceptionThrown;

        public event Action<PacketInfo> OnPacketReceived;

        public void Connect()
        {
            if (IsConnected)
            {
                return;
            }
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

        public void Disconnect()
        {
            if (!IsConnected)
            {
                return;
            }
            CancellationTokenSource.Cancel();
            Stream?.Dispose();
            Stream = null;
            Socket?.Close();
            Socket = null;
            IsConnected = false;
            lock (PendingResponses)
            {
                foreach (KeyValuePair<ushort, TaskCompletionSource<string>> kvp in PendingResponses)
                {
                    kvp.Value.TrySetException(new Exception("Disconnected before receiving the response."));
                }

                PendingResponses.Clear();
                PendingPartialResponses.Clear();
            }
        }

        public void Dispose()
        {
            Disconnect();
            CancellationTokenSource.Dispose();
        }

        public async Task<string> WriteCommandAsync(string command, CancellationToken cancellationToken)
        {
            ushort count = (ushort)(GetNextPacketId() & 0xFFFF);
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            lock (PendingResponses)
            {
                PendingResponses[count] = tcs;
                PendingPartialResponses[count] = new List<string>();
            }

            // 1) Enviar el comando real
            byte[] cmdPacket = EncodePacket(RconPacketType.MidPacketId, count, RconPacketType.ServerDataExecCommand, command);
            Stream.Write(cmdPacket, 0, cmdPacket.Length);

            // 2) Enviar un paquete vacío para forzar que el servidor envíe un paquete vacío de cierre
            byte[] emptyPacket = EncodePacket(RconPacketType.MidPacketId, count, RconPacketType.ServerDataResponseValue, "");
            Stream.Write(emptyPacket, 0, emptyPacket.Length);

            // Esperar respuesta completa
            using (cancellationToken.Register(() => tcs.TrySetCanceled()))
            {
                return await tcs.Task.ConfigureAwait(false);
            }
        }

        private static DecodedPacket DecodePacket(byte[] data)
        {
            if (data.Length < 14) // Tamaño mínimo esperado de un paquete
            {
                throw new Exception($"[ERROR] Paquete demasiado pequeño para ser válido. Tamaño: {data.Length}");
            }

            int size = BitConverter.ToInt32(data, 0);

            if (size <= 0 || size > 4096)
            {
                throw new Exception($"[ERROR] Tamaño inválido del paquete: {size}");
            }

            byte id = data[4];  // ID del paquete
            byte unused = data[5]; // Debe ser 0
            ushort count = BitConverter.ToUInt16(data, 6); // "Count"
            int type = BitConverter.ToInt32(data, 8); // "Type"

            // Asegurar alineación de `type`
            if (type < 0 || type > 10)
            {
                throw new Exception($"[ERROR] Type inválido: {type}");
            }

            int bodyLen = (size - 10); // Resta 10 bytes de encabezado (id=1, unused=1, count=2, type=4)
            if (bodyLen < 0) bodyLen = 0; // Seguridad para evitar valores negativos

            string body = Encoding.UTF8.GetString(data, 12, bodyLen);

            Console.WriteLine($"[DEBUG] Decoded Packet - Size: {size}, Id: {id}, Count: {count}, Type: {type}, Body: {body}");

            return new DecodedPacket { Size = size, Id = id, Count = count, Type = type, Body = body };
        }

        private static byte[] EncodePacket(byte id, ushort count, int type, string body)
        {
            int bodyLen = Encoding.UTF8.GetByteCount(body);
            int size = 14 + bodyLen;
            byte[] buf = new byte[size];
            BitConverter.TryWriteBytes(buf.AsSpan(0, 4), size - 4);
            buf[4] = id;
            buf[5] = 0;
            BitConverter.TryWriteBytes(buf.AsSpan(6, 2), count);
            BitConverter.TryWriteBytes(buf.AsSpan(8, 4), type);
            Encoding.UTF8.GetBytes(body, 0, body.Length, buf, 12);
            return buf;
        }

        private void Authenticate()
        {
            int authId = GetNextPacketId();
            byte[] pkt = EncodePacket(RconPacketType.MidPacketId, (ushort)authId, RconPacketType.ServerDataAuth, Password);
            Stream.Write(pkt, 0, pkt.Length);
            PacketInfo resp = ReadSinglePacketBlocking();
            if (resp.Id == -1)
            {
                throw new Exception("Authentication failed: invalid RCON password.");
            }
        }

        private byte[] ExtractRawPacket()
        {
            lock (IncomingBuffer)
            {
                if (IncomingBuffer.Count < 4) return null;

                int size = BitConverter.ToInt32(IncomingBuffer.ToArray(), 0);

                if (size < 10 || size > 4096)
                {
                    Console.WriteLine($"[ERROR] Tamaño de paquete inválido en el buffer: {size}");
                    IncomingBuffer.Clear();  // 🚀 Limpieza total si encontramos un paquete inválido
                    return null;
                }

                int totalSize = size + 4;
                if (IncomingBuffer.Count < totalSize) return null; // Esperar más datos si aún no tenemos el paquete completo

                // 🚀 Extraer paquete completo
                byte[] packet = IncomingBuffer.GetRange(0, totalSize).ToArray();

                // 🛑 Aquí eliminamos los bytes ya leídos para que no interfieran en la siguiente petición
                IncomingBuffer.RemoveRange(0, totalSize);

                return packet;
            }
        }

        private int GetNextPacketId()
        {
            int nextId = Interlocked.Increment(ref PacketIdCounter);
            Console.WriteLine($"[DEBUG] Generando nuevo PacketId: {nextId}");
            return nextId;
        }

        private async Task ListenForResponses(CancellationToken cancellationToken)
        {
            try
            {
                byte[] buffer = new byte[4096];
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (Stream == null || !Stream.CanRead)
                    {
                        throw new Exception("Stream is closed or not readable.");
                    }

                    int bytesRead = await Stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    if (bytesRead <= 0)
                    {
                        throw new Exception("Connection closed or unable to read from Stream.");
                    }

                    lock (IncomingBuffer)
                    {
                        IncomingBuffer.AddRange(buffer.AsSpan(0, bytesRead).ToArray());
                    }

                    OnBytesReceived?.Invoke(buffer.AsSpan(0, bytesRead).ToArray());
                    ParseIncomingBuffer();
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

        private void ParseIncomingBuffer()
        {
            while (true)
            {
                byte[] raw = ExtractRawPacket();
                if (raw == null) break;

                DecodedPacket dp = DecodePacket(raw);
                Console.WriteLine($"[DEBUG] dp.Id={dp.Id}, dp.Count={dp.Count}, dp.Type={dp.Type}, dp.Body='{dp.Body}'");

                lock (PendingResponses)
                {
                    // 🚨 Si no hay TCS activa para este Count, ignoramos paquetes vacíos adicionales
                    if (!PendingResponses.ContainsKey(dp.Count))
                    {
                        if (dp.Id == RconPacketType.MidPacketId && string.IsNullOrEmpty(dp.Body))
                        {
                            Console.WriteLine($"[DEBUG] Ignorando paquete vacío adicional para Count={dp.Count}.");
                        }
                        else
                        {
                            Console.WriteLine($"[DEBUG] Paquete inesperado con Count={dp.Count} sin una TCS activa. Se descarta.");
                        }
                        continue;
                    }

                    // Acumular respuesta
                    PendingPartialResponses[dp.Count].Add(dp.Body);

                    // Detectar si la respuesta ha finalizado
                    if (string.IsNullOrEmpty(dp.Body) && dp.Type == RconPacketType.ServerDataResponseValue)
                    {
                        Console.WriteLine($"[DEBUG] Detectado paquete vacío final para Count={dp.Count}, cerrando respuesta.");

                        string fullResponse = string.Concat(PendingPartialResponses[dp.Count]);
                        PendingResponses[dp.Count].TrySetResult(fullResponse);

                        PendingResponses.Remove(dp.Count);
                        PendingPartialResponses.Remove(dp.Count);

                        // 🔥 Marcar este `Count` como finalizado para ignorar paquetes rezagados
                        FinalizedResponses.Add(dp.Count);

                        // 🔥 Limpiar buffer después de procesar la respuesta
                        lock (IncomingBuffer)
                        {
                            Console.WriteLine("[DEBUG] Limpiando buffer después de una respuesta completa.");
                            IncomingBuffer.Clear();
                        }
                    }
                }
            }
        }

        private void ReadExact(byte[] buffer, int offset, int count)
        {
            int total = 0;
            while (total < count)
            {
                int r = Stream.Read(buffer, offset + total, count - total);
                if (r <= 0)
                {
                    throw new Exception("Connection closed or unable to read requested bytes.");
                }
                total += r;
            }
        }

        private PacketInfo ReadSinglePacketBlocking()
        {
            byte[] sizeBuffer = new byte[4];
            int read = Stream.Read(sizeBuffer, 0, 4);
            if (read < 4)
            {
                throw new Exception("Unable to read packet size for auth.");
            }
            int packetSize = BitConverter.ToInt32(sizeBuffer, 0);
            if (packetSize < 10)
            {
                return PacketInfo.Empty;
            }
            byte[] packetBuffer = new byte[packetSize];
            ReadExact(packetBuffer, 0, packetSize);
            return PacketInfo.Parse(packetBuffer);
        }
    }

    internal static class RconClientExtensions
    {
        public static byte[] ToArrayBytes(this string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
    }
}