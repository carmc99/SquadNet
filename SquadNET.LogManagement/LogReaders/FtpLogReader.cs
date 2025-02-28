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

        public FtpLogReader(IConfiguration configuration)
        {
            string host = configuration["LogReaders:Ftp:Host"];
            string user = configuration["LogReaders:Ftp:User"];
            string password = configuration["LogReaders:Ftp:Password"];
            RemoteFilePath = configuration["LogReaders:Ftp:RemoteFilePath"];
            FtpClient = new FtpClient(host, user, password); //TODO: Inject dependency
        }

        public async Task WatchAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                FtpClient.Connect();
                OnWatchStarted?.Invoke();

                // Initialize LastPosition when starting
                LastPosition = FtpClient.GetFileSize(RemoteFilePath);

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

                        long fileSize = FtpClient.GetFileSize(RemoteFilePath);
                        if (fileSize < LastPosition)
                        {
                            // File was truncated or replaced, restart from the beginning
                            LastPosition = 0;
                        }

                        using MemoryStream stream = new();
                        bool result = FtpClient.DownloadStream(stream, RemoteFilePath);

                        if (!result)
                        {
                            OnError?.Invoke("Failed to download log file.");
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
            FtpClient.Disconnect();
            OnWatchStopped?.Invoke();
            return Task.CompletedTask;
        }
    }
}
