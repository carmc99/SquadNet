using Renci.SshNet;

namespace SquadNET.LogManagement.LogReaders
{
    public class SftpLogReader : ILogReader
    {
        private readonly SftpClient SftpClient;
        private readonly string RemoteFilePath;

        public event Action<string> OnLogLine;

        public SftpLogReader(string host, string username, string password, string remoteFilePath)
        {
            SftpClient = new SftpClient(host, username, password);
            RemoteFilePath = remoteFilePath;
        }

        public async Task WatchAsync()
        {
            SftpClient.Connect();
            await Task.Run(async () =>
            {
                while (true)
                {
                    using (var stream = new MemoryStream())
                    {
                        SftpClient.DownloadFile(RemoteFilePath, stream);
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
            SftpClient.Disconnect();
            return Task.CompletedTask;
        }
    }
}
