using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Squadmania.Squad.Rcon;
using SquadNET.Core;
using SquadNET.Core.Squad.Commands;
using SquadNET.Core.Squad.Entities;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SquadNET.Rcon
{
    public class SquadRcon : IRconService
    {
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

        public SquadRcon(IConfiguration configuration, ILogger<SquadRcon> logger)
        {
            Configuration = configuration;
            Logger = logger;

            Host = Configuration["Rcon:Host"]
                ?? throw new ArgumentNullException("Rcon:Host is not defined in the configuration.");
            Port = int.TryParse(Configuration["Rcon:Port"], out int parsedPort) ?
                parsedPort : throw new ArgumentException("Rcon:Port must be a valid number.");
            Password = Configuration["Rcon:Password"]
                ?? throw new ArgumentNullException("Rcon:Password is not defined in the configuration.");
        }

        public void Connect()
        {
            try
            {
                if (IsConnected)
                {
                    return;
                }

                RconClient = new RconClient(new IPEndPoint(
                   IPAddress.Parse(Host), Port),
                   Password);

                // Subscribe to RconClient events
                RconClient.Connected += () =>
                {
                    OnConnected?.Invoke();
                };
                RconClient.PacketReceived += packet =>
                {
                    OnPacketReceived?.Invoke(PacketInfo.Convert(packet));
                };
                RconClient.ChatMessageReceived += message =>
                {
                    OnChatMessageReceived?.Invoke(ChatMessageInfo.Convert(message));
                };
                RconClient.ExceptionThrown += exception =>
                {
                    OnExceptionThrown?.Invoke(exception);
                };
                RconClient.BytesReceived += bytes =>
                {
                    OnBytesReceived?.Invoke(bytes);
                };

                RconClient.Start();
                IsConnected = true;
                Logger.LogInformation("Successfully connected to the RCON server.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error connecting to the RCON server.");
                throw;
            }
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                RconClient.Stop();
                IsConnected = false;
                Logger.LogInformation("Disconnected from the RCON server.");
            }
        }

        public async Task<string> ExecuteCommandAsync<SquadCommand>(Command<SquadCommand> command, SquadCommand commandType, params object[] args) where SquadCommand : Enum
        {
            try
            {
                Connect();

                if (!IsConnected)
                {
                    Logger.LogWarning("Attempted to execute an RCON command without a connection.");
                    throw new InvalidOperationException("Cannot execute command because there is no active connection.");
                }

                string formattedCommand = command.GetFormattedCommand(commandType, args);
                byte[] responseBytes = await RconClient.WriteCommandAsync(formattedCommand, CancellationToken.None);

                string response = System.Text.Encoding.UTF8.GetString(responseBytes);

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
                //Disconnect(); //TODO: Review exception handling
            }
        }
    }
}
