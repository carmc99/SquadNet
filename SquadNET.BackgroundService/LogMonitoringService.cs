using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SquadNET.LogManagement;
using Microsoft.Extensions.Hosting;
using MediatR;
using SquadNET.Application.Services;
using SquadNET.Application.Squad.ParseLine;
using SquadNET.Core;


namespace SquadNET.Services
{
    public class LogMonitoringService : BackgroundService
    {
        private readonly ILogger<LogMonitoringService> Logger;
        private readonly ILogReaderFactory LogReaderFactory;
        private readonly IConfiguration Configuration;
        private readonly PluginManager PluginManager;
        private readonly IMediator Mediator;
        private readonly IRconService RconService;

        private ILogReader LogReader;

        public LogMonitoringService(
            ILogger<LogMonitoringService> logger,
            ILogReaderFactory logReaderFactory,
            IConfiguration configuration,
            PluginManager pluginManager,
            IMediator mediator,
            IRconService rconService)
        {
            Logger = logger;
            LogReaderFactory = logReaderFactory;
            Configuration = configuration;
            PluginManager = pluginManager;
            Mediator = mediator;
            RconService = rconService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            LogReaderType logReaderType = Enum.Parse<LogReaderType>(Configuration["LogReaders:Type"]);
            LogReader = LogReaderFactory.Create(logReaderType);

            RconService.Connect();

            RconService.OnBytesReceived += OnByteReceived;
            LogReader.OnError += error => Logger.LogError($"Log Reader Error: {error}");
            LogReader.OnFileDeleted += () => Logger.LogWarning("Log file was deleted.");
            LogReader.OnFileCreated += () => Logger.LogInformation("Log file was created.");
            LogReader.OnFileRenamed += () => Logger.LogInformation("Log file was renamed.");
            LogReader.OnWatchStarted += () => Logger.LogInformation("Log monitoring started.");
            LogReader.OnWatchStopped += () => Logger.LogInformation("Log monitoring stopped.");
            LogReader.OnLogLine += OnLogLineReceived;

            await LogReader.WatchAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

            RconService.Disconnect();
            await LogReader.UnwatchAsync();
        }

        private void OnByteReceived(byte[] bytes)
        {
            Logger.LogInformation($"{BitConverter.ToString(bytes)}");
        }
        private async void OnLogLineReceived(string line)
        {
            Logger.LogInformation("LogLine: {Line}", line);
            ParseLineQueryHandler.Response result = await Mediator.Send(new ParseLineQueryHandler.Request
            {
                Line = line,
            });
            if (result != null)
            {
                PluginManager.EmitEvent(result.EventName, result.EventData);
            }
        }
    }
}
