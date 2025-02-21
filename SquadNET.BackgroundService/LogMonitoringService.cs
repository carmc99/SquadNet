using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SquadNET.LogManagement;
using Microsoft.Extensions.Hosting;


namespace SquadNET.Services
{
    public class LogMonitoringService : BackgroundService
    {
        private readonly ILogger<LogMonitoringService> Logger;
        private readonly ILogReaderFactory LogReaderFactory;
        private readonly IConfiguration Configuration;
        private ILogReader LogReader;

        public LogMonitoringService(ILogger<LogMonitoringService> logger, ILogReaderFactory logReaderFactory, IConfiguration configuration)
        {
            Logger = logger;
            LogReaderFactory = logReaderFactory;
            Configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            LogReaderType logReaderType = Enum.Parse<LogReaderType>(Configuration["LogReaders:Type"]);
            LogReader = LogReaderFactory.Create(logReaderType);

            LogReader.OnLogLine += line => Logger.LogInformation($"New Log Line: {line}");
            LogReader.OnError += error => Logger.LogError($"Log Reader Error: {error}");
            LogReader.OnFileDeleted += () => Logger.LogWarning("Log file was deleted.");
            LogReader.OnFileCreated += () => Logger.LogInformation("Log file was created.");
            LogReader.OnFileRenamed += () => Logger.LogInformation("Log file was renamed.");
            LogReader.OnWatchStarted += () => Logger.LogInformation("Log monitoring started.");
            LogReader.OnWatchStopped += () => Logger.LogInformation("Log monitoring stopped.");

            await LogReader.WatchAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            await LogReader.UnwatchAsync();
        }
    }
}
