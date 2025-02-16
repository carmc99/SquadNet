
namespace SquadNET.LogManagement.LogReaders
{
    public class TailLogReader : ILogReader
    {
        private readonly string FilePath;
        private FileSystemWatcher Watcher;

        public event Action<string> OnLogLine;

        public TailLogReader(string filePath)
        {
            FilePath = filePath;
        }

        public async Task WatchAsync()
        {
            Watcher = new FileSystemWatcher(Path.GetDirectoryName(FilePath))
            {
                Filter = Path.GetFileName(FilePath)
            };
            Watcher.Changed += async (s, e) => await ReadNewLinesAsync();
            Watcher.EnableRaisingEvents = true;
        }

        private async Task ReadNewLinesAsync()
        {
            using (FileStream stream = new(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        OnLogLine?.Invoke(line);
                    }
                }
            }
        }

        public Task UnwatchAsync()
        {
            Watcher?.Dispose();
            return Task.CompletedTask;
        }
    }
}
