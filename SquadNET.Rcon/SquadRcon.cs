﻿using Microsoft.Extensions.Configuration;
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

        public event Action Connected;
        public event Action<Core.Squad.Packet> PacketReceived;
        public event Action<ChatMessageInfo> ChatMessageReceived;
        public event Action<Exception> ExceptionThrown;
        public event Action<byte[]> BytesReceived;

        private CancellationTokenSource ListeningCancellationTokenSource;


        public SquadRcon(IConfiguration configuration, ILogger<SquadRcon> logger)
        {
            Configuration = configuration;
            Logger = logger;

            Host = Configuration["Rcon:Host"]
                ?? throw new ArgumentNullException("Rcon:Host no está definido en la configuración.");
            Port = int.TryParse(Configuration["Rcon:Port"], out int parsedPort) ? 
                parsedPort : throw new ArgumentException("Rcon:Port debe ser un número válido.");
            Password = Configuration["Rcon:Password"]
                ?? throw new ArgumentNullException("Rcon:Password no está definido en la configuración.");

           
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

                // Suscribir eventos de RconClient
                RconClient.Connected += () =>
                {
                    Logger.LogInformation("Conectado correctamente al servidor RCON.");
                    Connected?.Invoke();
                };
                //TODO:Complete
                //RconClient.PacketReceived += packet =>
                //{
                //    Logger.LogDebug($"Paquete recibido: {packet}");
                //    PacketReceived?.Invoke(Squadmania.Squad.Rcon.Packet);
                //};
                //RconClient.ChatMessageReceived += message =>
                //{
                //    Logger.LogInformation($"Mensaje de chat recibido: {message.Message}");
                //    ChatMessageReceived?.Invoke(message);
                //};
                RconClient.ExceptionThrown += exception =>
                {
                    Logger.LogError(exception, "Excepción en RconClient");
                    ExceptionThrown?.Invoke(exception);
                };
                RconClient.BytesReceived += bytes =>
                {
                    Logger.LogDebug($"Bytes recibidos: {BitConverter.ToString(bytes)}");
                    BytesReceived?.Invoke(bytes);
                };

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

        public async Task<string> ExecuteCommandAsync<SquadCommand>(Command<SquadCommand> command, SquadCommand commandType, params object[] args) where SquadCommand : Enum
        {

            try
            {
                Connect();

                if (!IsConnected)
                {
                    Logger.LogWarning("Intento de ejecutar un comando RCON sin conexión.");
                    throw new InvalidOperationException("No se puede ejecutar el comando porque no hay conexión activa.");
                }

                string formattedCommand = command.GetFormattedCommand(commandType, args);
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
            finally
            {
                //Disconnect(); //TODO: Revisar excepcion
            }
        }
    }
}
