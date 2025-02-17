using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Rcon
{
    public class SquadRcon : IRconService
    {
        private readonly IConfiguration Configuration;
        private TcpClient Client;
        private NetworkStream Stream;
        private readonly string Host;
        private readonly int Port;
        private readonly string Password;

        public SquadRcon(IConfiguration configuration)
        {
            Configuration = configuration;
            Host = Configuration["Rcon:Host"];
            Port = int.Parse(Configuration["Rcon:Port"]);
            Password = Configuration["Rcon:Password"];
        }

        public async Task ConnectAsync()
        {
            Client = new TcpClient(); // TODO: Inyectar?
            await Client.ConnectAsync(Host, Port);
            Stream = Client.GetStream();
            await AuthenticateAsync();
        }

        private async Task AuthenticateAsync()
        {
            await SendCommandAsync(3, Password); // Auth Command
        }

        public async Task DisconnectAsync()
        {
            Client?.Close();
        }

        public async Task<string> ExecuteCommandAsync(RconCommand command, params object[] args)
        {
            string formattedCommand = command.GetFormattedCommand(args);
            return await SendCommandAsync(2, formattedCommand);
        }

        private async Task<string> SendCommandAsync(int type, string body)
        {
            byte[] buffer = new byte[4096];
            byte[] request = EncodePacket(type, body);
            await Stream.WriteAsync(request);
            int responseLength = await Stream.ReadAsync(buffer);
            return DecodePacket(buffer, responseLength);
        }

        private byte[] EncodePacket(int type, string body)
        {
            int size = body.Length + 14;
            byte[] buffer = new byte[size];
            BitConverter.GetBytes(size - 4).CopyTo(buffer, 0);
            BitConverter.GetBytes(type).CopyTo(buffer, 8);
            Encoding.UTF8.GetBytes(body).CopyTo(buffer, 12);
            return buffer;
        }

        private string DecodePacket(byte[] buffer, int length)
        {
            return Encoding.UTF8.GetString(buffer, 12, length - 14);
        }
    }
}
