// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Rcon;
using System.Net;
using System.Text;

public class SquadRcon : IRconService, IDisposable
{
    private readonly IConfiguration Configuration;
    private readonly string Host;
    private readonly ILogger<SquadRcon> Logger;
    private readonly IParser<ChatMessageInfo> Parser;
    private readonly string Password;
    private readonly int Port;
    private readonly RconClient RconClient;
    private bool IsConnected;

    public SquadRcon(IConfiguration configuration, ILogger<SquadRcon> logger, IParser<ChatMessageInfo> parser)
    {
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Parser = parser ?? throw new ArgumentNullException(nameof(parser));

        Host = Configuration["Rcon:Host"]
            ?? throw new ArgumentNullException("Rcon:Host is not defined in the configuration.");

        if (!int.TryParse(Configuration["Rcon:Port"], out int parsedPort))
        {
            throw new ArgumentException("Rcon:Port must be a valid number.");
        }
        Port = parsedPort;

        Password = Configuration["Rcon:Password"]
            ?? throw new ArgumentNullException("Rcon:Password is not defined in the configuration.");

        RconClient = new RconClient(new IPEndPoint(IPAddress.Parse(Host), Port), Password);

        RconClient.OnConnected += () => { OnConnected?.Invoke(); };
        RconClient.OnPacketReceived += packet => { ProcessPacket(packet); };
        RconClient.OnExceptionThrown += exception => { OnExceptionThrown?.Invoke(exception); };
        RconClient.OnBytesReceived += bytes => { OnBytesReceived?.Invoke(bytes); };

        Connect();
    }

    public event Action<byte[]> OnBytesReceived;

    public event Action<ChatMessageInfo> OnChatMessageReceived;

    public event Action OnConnected;

    public event Action<Exception> OnExceptionThrown;

    public event Action<PacketInfo> OnPacketReceived;

    public void Connect()
    {
        try
        {
            if (IsConnected) return;

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

    public void Dispose()
    {
        if (IsConnected)
        {
            RconClient.Disconnect();
            IsConnected = false;
            Logger.LogInformation("Disconnected from the RCON server.");
        }
    }

    public async Task<string> ExecuteCommandAsync<SquadCommand>(Command<SquadCommand> command, SquadCommand commandType, params object[] args) where SquadCommand : Enum
    {
        try
        {
            if (!IsConnected)
            {
                Logger.LogWarning("Attempted to execute an RCON command without a connection.");
                throw new InvalidOperationException("No active RCON connection.");
            }

            string formattedCommand = command.GetFormattedCommand(commandType, args);
            string response = await RconClient.WriteCommandAsync(formattedCommand, CancellationToken.None);

            Logger.LogInformation($"Command executed: {formattedCommand} | Response: {response}");
            return response;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"Error executing RCON command: {command}");
            throw;
        }
    }

    private void ProcessPacket(PacketInfo packet)
    {
        if (packet.Id == 4 && packet.Type == RconPacketType.ServerDataExecCommand && packet.Body.Length == 0)
        {
            return;
        }

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