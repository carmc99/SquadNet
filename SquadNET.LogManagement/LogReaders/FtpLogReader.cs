using FluentFTP;

namespace SquadNET.LogManagement.LogReaders
{
    public class FtpLogReader : ILogReader
    {
        private readonly FtpClient FtpClient;
        private readonly string RemoteFilePath;

        public event Action<string> OnLogLine;

        public FtpLogReader(string host, string username, string password, string remoteFilePath)
        {
            FtpClient = new FtpClient(host, username, password);
            RemoteFilePath = remoteFilePath;
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
