using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using SquadNET.Core;

namespace SquadNET.LogManagement.LogReaders
{
    public class SftpLogReader : ILogReader
    {
        private readonly SftpClient SftpClient;
        private readonly string RemoteFilePath;
        private long LastPosition = 0; // Tracks the last read position

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
            int port = 22;
            configuration["LogReaders:Sftp:Port"].TryParse(out port);
            string user = configuration["LogReaders:Sftp:User"];
            string password = configuration["LogReaders:Sftp:Password"];
            RemoteFilePath = configuration["LogReaders:Sftp:RemoteFilePath"];
            SftpClient = new SftpClient(host, port, user, password); //TODO: Inject dependency
        }

        public async Task WatchAsync()
        {
            try
            {
                SftpClient.Connect();
                OnWatchStarted?.Invoke();

                LastPosition = SftpClient.GetAttributes(RemoteFilePath).Size;

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

                        long fileSize = SftpClient.GetAttributes(RemoteFilePath).Size;
                        if (fileSize < LastPosition)
                        {
                            // File was truncated or replaced, restart from the beginning
                            LastPosition = 0;
                        }

                        using MemoryStream stream = new();
                        try
                        {
                            SftpClient.DownloadFile(RemoteFilePath, stream);
                        }
                        catch (Exception ex)
                        {
                            OnError?.Invoke($"Failed to download log file: {ex.Message}");
                            await Task.Delay(5000);
                            continue;
                        }

                        stream.Position = LastPosition; // Move to last read position
                        using StreamReader reader = new(stream);

                        while (!reader.EndOfStream)
                        {
                            string line = await reader.ReadLineAsync();
                            if (!string.IsNullOrWhiteSpace(line)) // Ignore empty lines
                            {
                                OnLogLine?.Invoke(line);
                            }
                        }

                        // Update LastPosition to prevent re-reading old lines
                        LastPosition = stream.Length;

                        await Task.Delay(5000);
                    }
                });
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error in WatchAsync: {ex.Message}");
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