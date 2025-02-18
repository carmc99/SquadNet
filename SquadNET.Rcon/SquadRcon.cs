using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Squadmania.Squad.Rcon;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SquadNET.Rcon
{
    public class SquadRcon : IRconService
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<SquadRcon> Logger;
        private readonly RconClient RconClient;
        private bool IsConnected;

        public SquadRcon(IConfiguration configuration, ILogger<SquadRcon> logger)
        {
            Configuration = configuration;
            Logger = logger;

            string host = Configuration["Rcon:Host"]
                ?? throw new ArgumentNullException("Rcon:Host no está definido en la configuración.");
            int port = int.TryParse(Configuration["Rcon:Port"], out int parsedPort) ? 
                parsedPort : throw new ArgumentException("Rcon:Port debe ser un número válido.");
            string password = Configuration["Rcon:Password"]
                ?? throw new ArgumentNullException("Rcon:Password no está definido en la configuración.");

            RconClient = new RconClient(new IPEndPoint(
                IPAddress.Parse(host), port),
                password);
        }

        public void Connect()
        {
            try
            {
                RconClient.Start();
                IsConnected = true;
                Logger.LogInformation("Conectado correctamente al servidor RCON.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error al conectar con el servidor RCON.");
                throw;
            }
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                RconClient.Stop();
                IsConnected = false;
                Logger.LogInformation("Desconectado del servidor RCON.");
            }
        }

        public async Task<string> ExecuteCommandAsync(RconCommand command, params object[] args)
        {
            if (!IsConnected)
            {
                Logger.LogWarning("Intento de ejecutar un comando RCON sin conexión.");
                throw new InvalidOperationException("No se puede ejecutar el comando porque no hay conexión activa.");
            }

            try
            {
                string formattedCommand = command.GetFormattedCommand(args);
                byte[] responseBytes = await RconClient.WriteCommandAsync(formattedCommand, CancellationToken.None);
               
                string response = System.Text.Encoding.UTF8.GetString(responseBytes);

                Logger.LogInformation($"Comando ejecutado: {formattedCommand} | Respuesta: {response}");

                return response;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error ejecutando comando RCON: {command}");
                throw;
            }
        }
    }
}
