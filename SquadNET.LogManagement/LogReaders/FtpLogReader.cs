using FluentFTP;
using Microsoft.Extensions.Configuration;

namespace SquadNET.LogManagement.LogReaders
{
    public class FtpLogReader : ILogReader
    {
        private readonly FtpClient FtpClient;
        private readonly string RemoteFilePath;

        public event Action<string> OnLogLine;

        public FtpLogReader(IConfiguration configuration)
        {
            string host = configuration["LogReaders:Ftp:Host"];
            string user = configuration["LogReaders:Ftp:User"];
            string password = configuration["LogReaders:Ftp:Password"];
            RemoteFilePath = configuration["LogReaders:Ftp:RemoteFilePath"];
            FtpClient = new FtpClient(host, user, password); //TODO: Inyectar 
        }

        public async Task WatchAsync()
        {
            FtpClient.Connect();
            await Task.Run(async () =>
            {
                while (true)
                {
                    using (var stream = new MemoryStream())
                    {
                        bool result = FtpClient.DownloadStream(stream, RemoteFilePath);
                        stream.Position = 0;
                        using (StreamReader reader = new(stream))
                        {
                            while (!reader.EndOfStream)
                            {
                                var line = await reader.ReadLineAsync();
                                OnLogLine?.Invoke(line);
                            }
                        }
                    }
                    await Task.Delay(5000);
                }
            });
        }

        public Task UnwatchAsync()
        {
            FtpClient.Disconnect();
            return Task.CompletedTask;
        }
    }
}
