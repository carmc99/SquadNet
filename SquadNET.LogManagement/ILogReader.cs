using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.LogManagement
{
    /// <summary>
    /// Defines the contract for a log reader that monitors log files 
    /// or remote log sources and triggers events upon changes.
    /// </summary>
    public interface ILogReader
    {
        /// <summary>
        /// Event triggered when a new line is detected in the log.
        /// </summary>
        event Action<string> OnLogLine;

        /// <summary>
        /// Event triggered when an error occurs while reading the log.
        /// </summary>
        event Action<string> OnError;

        /// <summary>
        /// Event triggered when the log file is deleted. (Applicable to TailLogger)
        /// </summary>
        event Action OnFileDeleted;

        /// <summary>
        /// Event triggered when the log file is created. (Applicable to TailLogger)
        /// </summary>
        event Action OnFileCreated;

        /// <summary>
        /// Event triggered when the log file is renamed. (Applicable to TailLogger)
        /// </summary>
        event Action OnFileRenamed;

        /// <summary>
        /// Event triggered when the connection to a remote log source is lost.
        /// (Applicable to SftpLogger and FtpLogger)
        /// </summary>
        event Action OnConnectionLost;

        /// <summary>
        /// Event triggered when the connection to a remote log source is restored.
        /// (Applicable to SftpLogger and FtpLogger)
        /// </summary>
        event Action OnConnectionRestored;

        /// <summary>
        /// Event triggered when log monitoring starts.
        /// </summary>
        event Action OnWatchStarted;

        /// <summary>
        /// Event triggered when log monitoring stops.
        /// </summary>
        event Action OnWatchStopped;

        /// <summary>
        /// Starts monitoring the log for changes.
        /// </summary>
        /// <returns>A task that represents the asynchronous watch operation.</returns>
        Task WatchAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Stops monitoring the log.
        /// </summary>
        /// <returns>A task that represents the asynchronous unwatch operation.</returns>
        Task UnwatchAsync();
    }
}
