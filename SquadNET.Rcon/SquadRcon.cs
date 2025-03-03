using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SquadNET.Core;
using SquadNET.Core.Squad.Commands;
using SquadNET.Core.Squad.Entities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SquadNET.Rcon
{
    public class SquadRcon : IRconService
    {
        private readonly IParser<ChatMessageInfo> Parser;
        private readonly IConfiguration Configuration;
        private readonly ILogger<SquadRcon> Logger;
        private RconClient RconClient;
        private bool IsConnected;
        private readonly string Host;
        private readonly int Port;
        private readonly string Password;

        public event Action OnConnected;
        public event Action<PacketInfo> OnPacketReceived;
        public event Action<ChatMessageInfo> OnChatMessageReceived;
        public event Action<Exception> OnExceptionThrown;
        public event Action<byte[]> OnBytesReceived;

        public SquadRcon(IConfiguration configuration,
            ILogger<SquadRcon> logger,
            IParser<ChatMessageInfo> parser)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Host = Configuration["Rcon:Host"]
                ?? throw new ArgumentNullException("Rcon:Host is not defined in the configuration.");
            if (!int.TryParse(Configuration["Rcon:Port"], out int parsedPort))
            {
                throw new ArgumentException("Rcon:Port must be a valid number.");
            }
            Port = parsedPort;

            Password = Configuration["Rcon:Password"]
                ?? throw new ArgumentNullException("Rcon:Password is not defined in the configuration.");
            Parser = parser;
        }

        /// <summary>
        /// Establishes a connection to the RCON server if not already connected.
        /// Subscribes to the new RconClient's events.
        /// </summary>
        public void Connect()
        {
            try
            {
                if (IsConnected) return;

                RconClient = new RconClient(
                    new IPEndPoint(IPAddress.Parse(Host), Port),
                    Password
                );

                // Subscribe to new RconClient events
                RconClient.OnConnected += () =>
                {
                    OnConnected?.Invoke();
                };
                RconClient.OnPacketReceived += packet =>
                {
                    ProcessPacket(packet);
                };
                RconClient.OnExceptionThrown += exception =>
                {
                    OnExceptionThrown?.Invoke(exception);
                };
                RconClient.OnBytesReceived += bytes =>
                {
                    OnBytesReceived?.Invoke(bytes);
                };

                // Connect to the RCON server
                RconClient.Connect();
                IsConnected = true;

                Logger.LogInformation("Successfully connected to the RCON server.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error connecting to the RCON server.");
                throw;
            }
        }

        /// <summary>
        /// Disconnects from the RCON server if currently connected.
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected)
            {
                RconClient.Disconnect();
                IsConnected = false;
                Logger.LogInformation("Disconnected from the RCON server.");
            }
        }

        /// <summary>
        /// Executes an RCON command asynchronously.
        /// </summary>
        /// <typeparam name="SquadCommand">The enumeration type representing the command.</typeparam>
        /// <param name="command">The command template used for execution.</param>
        /// <param name="commandType">The specific command enum value to be executed.</param>
        /// <param name="args">Optional arguments for the command.</param>
        /// <returns>The server's response as a string.</returns>
        public async Task<string> ExecuteCommandAsync<SquadCommand>(
            Command<SquadCommand> command,
            SquadCommand commandType,
            params object[] args
        ) where SquadCommand : Enum
        {
            try
            {
                Connect();

                if (!IsConnected)
                {
                    Logger.LogWarning("Attempted to execute an RCON command without a connection.");
                    throw new InvalidOperationException("No active RCON connection.");
                }

                string formattedCommand = command.GetFormattedCommand(commandType, args);
                byte[] responseBytes = await RconClient.WriteCommandAsync(
                    formattedCommand,
                    CancellationToken.None
                );

                string response = Encoding.UTF8.GetString(responseBytes);
                Logger.LogInformation($"Command executed: {formattedCommand} | Response: {response}");

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error executing RCON command: {command}");
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Processes incoming packets, specifically looking for chat messages.
        /// </summary>
        /// <param name="packet">The received RCON packet.</param>
        private void ProcessPacket(PacketInfo packet)
        {
            // Squad-specific filtering for broken packets
            // The Squad server sometimes sends an invalid empty exec command packet.
            // These packets have Id: 4, Type: 2 (ServerDataExecCommand), and an empty body.
            // These packets should be ignored as they have no useful data.
            if (packet.Id == 4 && packet.Type == RconPacketType.ServerDataExecCommand && packet.Body.Length == 0)
            {
                return;
            }

            // Filter out unnecessary empty response packets
            if (packet is { Id: 3, Type: RconPacketType.ServerDataResponseValue })
            {
                return;
            }

            try
            {
                OnPacketReceived?.Invoke(packet);
            }
            catch (Exception e)
            {
                OnExceptionThrown?.Invoke(e);
            }

            if (packet.Type == RconPacketType.ServerDataChatMessage)
            {
                string rawMessage = Encoding.UTF8.GetString(packet.Body);

                ChatMessageInfo chatMessage = Parser.Parse(rawMessage);

                if (chatMessage != null)
                {
                    try
                    {
                        OnChatMessageReceived?.Invoke(chatMessage);
                    }
                    catch (Exception e)
                    {
                        OnExceptionThrown?.Invoke(e);
                    }
                }
            }
        }
    }
}
