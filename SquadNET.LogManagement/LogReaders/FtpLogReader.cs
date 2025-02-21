using System;
using System.IO;
using System.Threading.Tasks;
using FluentFTP;
using Microsoft.Extensions.Configuration;

namespace SquadNET.LogManagement.LogReaders
{
    public class FtpLogReader : ILogReader
    {
        private readonly FtpClient FtpClient;
        private readonly string RemoteFilePath;

        public event Action<string> OnLogLine;
        public event Action<string> OnError;
        public event Action OnFileDeleted;
        public event Action OnFileCreated;
        public event Action OnFileRenamed;
        public event Action OnConnectionLost;
        public event Action OnConnectionRestored;
        public event Action OnWatchStarted;
        public event Action OnWatchStopped;

        public FtpLogReader(IConfiguration configuration)
        {
            string host = configuration["LogReaders:Ftp:Host"];
            string user = configuration["LogReaders:Ftp:User"];
            string password = configuration["LogReaders:Ftp:Password"];
            RemoteFilePath = configuration["LogReaders:Ftp:RemoteFilePath"];
            FtpClient = new FtpClient(host, user, password); //TODO: Inject dependency
        }

        public async Task WatchAsync()
        {
            try
            {
                FtpClient.Connect();
                OnWatchStarted?.Invoke();

                await Task.Run(async () =>
                {
                    while (true)
                    {
                        if (!FtpClient.IsConnected)
                        {
                            OnConnectionLost?.Invoke();
                            FtpClient.Connect();
                            OnConnectionRestored?.Invoke();
                        }

                        using (MemoryStream stream = new())
                        {
                            bool result = FtpClient.DownloadStream(stream, RemoteFilePath);

                            if (!result)
                            {
                                OnError?.Invoke("Failed to download log file.");
                                continue;
                            }

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
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        public Task UnwatchAsync()
        {
            FtpClient.Disconnect();
            OnWatchStopped?.Invoke();
            return Task.CompletedTask;
        }
    }
}