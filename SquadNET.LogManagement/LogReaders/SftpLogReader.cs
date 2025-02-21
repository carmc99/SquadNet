using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;

namespace SquadNET.LogManagement.LogReaders
{
    public class SftpLogReader : ILogReader
    {
        private readonly SftpClient SftpClient;
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

        public SftpLogReader(IConfiguration configuration)
        {
            string host = configuration["LogReaders:Sftp:Host"];
            string user = configuration["LogReaders:Sftp:User"];
            string password = configuration["LogReaders:Sftp:Password"];
            RemoteFilePath = configuration["LogReaders:Sftp:RemoteFilePath"];
            SftpClient = new SftpClient(host, user, password); //TODO: Inject dependency
        }

        public async Task WatchAsync()
        {
            try
            {
                SftpClient.Connect();
                OnWatchStarted?.Invoke();

                await Task.Run(async () =>
                {
                    while (true)
                    {
                        if (!SftpClient.IsConnected)
                        {
                            OnConnectionLost?.Invoke();
                            SftpClient.Connect();
                            OnConnectionRestored?.Invoke();
                        }

                        using MemoryStream stream = new();
                        try
                        {
                            SftpClient.DownloadFile(RemoteFilePath, stream);
                        }
                        catch (Exception ex)
                        {
                            OnError?.Invoke($"Failed to download log file: {ex.Message}");
                            continue;
                        }

                        stream.Position = 0;
                        using StreamReader reader = new(stream);
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            OnLogLine?.Invoke(line);
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
            SftpClient.Disconnect();
            OnWatchStopped?.Invoke();
            return Task.CompletedTask;
        }
    }
}
