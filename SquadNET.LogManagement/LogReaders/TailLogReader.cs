using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SquadNET.LogManagement.LogReaders
{
    public class TailLogReader : ILogReader
    {
        private readonly string FilePath;
        private FileSystemWatcher Watcher;

        public event Action<string> OnLogLine;
        public event Action<string> OnError;
        public event Action OnFileDeleted;
        public event Action OnFileCreated;
        public event Action OnFileRenamed;
        // Not used in TailLogReader
        public event Action OnConnectionLost;

        // Not used in TailLogReader
        public event Action OnConnectionRestored;
        public event Action OnWatchStarted;
        public event Action OnWatchStopped;

        public TailLogReader(IConfiguration configuration)
        {
            FilePath = configuration["LogReaders:Tail:FilePath"];
        }

        public async Task WatchAsync()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    OnFileCreated?.Invoke();
                }

                Watcher = new FileSystemWatcher(Path.GetDirectoryName(FilePath))
                {
                    Filter = Path.GetFileName(FilePath),
                    EnableRaisingEvents = true
                };

                Watcher.Changed += async (s, e) => await ReadNewLinesAsync();
                Watcher.Deleted += (s, e) => OnFileDeleted?.Invoke();
                Watcher.Renamed += (s, e) => OnFileRenamed?.Invoke();

                OnWatchStarted?.Invoke();
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        private async Task ReadNewLinesAsync()
        {
            try
            {
                using FileStream stream = new(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader reader = new(stream);
                while (!reader.EndOfStream)
                {
                    string line = await reader.ReadLineAsync();
                    OnLogLine?.Invoke(line);
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        public Task UnwatchAsync()
        {
            Watcher?.Dispose();
            OnWatchStopped?.Invoke();
            return Task.CompletedTask;
        }
    }
}
