// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.Configuration;

namespace SquadNET.LogManagement.LogReaders
{
    public class TailLogReader : ILogReader
    {
        private readonly string FilePath;
        private long LastPosition = 0;
        private FileSystemWatcher Watcher;

        public TailLogReader(IConfiguration configuration)
        {
            FilePath = configuration["LogReaders:Tail:FilePath"];
        }

        // Not used in TailLogReader
        public event Action OnConnectionLost;

        // Not used in TailLogReader
        public event Action OnConnectionRestored;

        public event Action<string> OnError;

        public event Action OnFileCreated;

        public event Action OnFileDeleted;

        public event Action OnFileRenamed;

        public event Action<string> OnLogLine;

        public event Action OnWatchStarted;

        public event Action OnWatchStopped;

        public Task UnwatchAsync()
        {
            Watcher?.Dispose();
            OnWatchStopped?.Invoke();
            return Task.CompletedTask;
        }

        public async Task WatchAsync(CancellationToken cancellationToken = default)
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
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                    EnableRaisingEvents = true
                };

                Watcher.Changed += async (s, e) => await ReadNewLinesAsync();
                Watcher.Deleted += (s, e) => OnFileDeleted?.Invoke();
                Watcher.Renamed += (s, e) => OnFileRenamed?.Invoke();

                OnWatchStarted?.Invoke();

                // Initialize the last position from the existing file
                LastPosition = new FileInfo(FilePath).Length;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error in WatchAsync: {ex.Message}");
            }
        }

        private async Task ReadNewLinesAsync()
        {
            try
            {
                using FileStream stream = new(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                stream.Seek(LastPosition, SeekOrigin.Begin); // Move to last read position
                using StreamReader reader = new(stream);

                while (!reader.EndOfStream)
                {
                    string line = await reader.ReadLineAsync();

                    if (!string.IsNullOrWhiteSpace(line)) // Ignore empty lines
                    {
                        OnLogLine?.Invoke(line);
                    }
                }

                // Update last read position
                LastPosition = stream.Position;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error in ReadNewLinesAsync: {ex.Message}");
            }
        }
    }
}