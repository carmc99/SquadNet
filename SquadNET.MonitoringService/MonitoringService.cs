using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SquadNET.LogManagement;
using Microsoft.Extensions.Hosting;
using MediatR;
using SquadNET.Application.Services;
using SquadNET.Application.Squad.ParseLine;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using System.Text;
using SquadNET.Core.Squad.Models;


namespace SquadNET.MonitoringService
{
    public class MonitoringService : BackgroundService
    {
        private readonly ILogger<MonitoringService> Logger;
        private readonly ILogReaderFactory LogReaderFactory;
        private readonly IConfiguration Configuration;
        private readonly PluginManager PluginManager;
        private readonly IMediator Mediator;
        private readonly IRconService RconService;
        private readonly ILogReader LogReaderService;

        public MonitoringService(
            ILogger<MonitoringService> logger,
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
            LogReaderType logReaderType = Enum.Parse<LogReaderType>(Configuration["LogReaders:Type"]);
            LogReaderService = LogReaderFactory.Create(logReaderType);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RconService.Connect();

            RconService.OnPacketReceived += OnPacketReceived;
            LogReaderService.OnError += error => Logger.LogError($"Log Reader Error: {error}");
            LogReaderService.OnFileDeleted += () => Logger.LogWarning("Log file was deleted.");
            LogReaderService.OnFileCreated += () => Logger.LogInformation("Log file was created.");
            LogReaderService.OnFileRenamed += () => Logger.LogInformation("Log file was renamed.");
            LogReaderService.OnWatchStarted += () => Logger.LogInformation("Monitoring started.");
            LogReaderService.OnWatchStopped += () => Logger.LogInformation("Monitoring stopped.");
            LogReaderService.OnLogLine += OnLogLineReceived;

            await LogReaderService.WatchAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await LogReaderService.UnwatchAsync();
            RconService.Disconnect();
            await base.StopAsync(stoppingToken);
        }

        private void OnPacketReceived(PacketInfo message)
        {
            string text = Encoding.UTF8.GetString(message.Body);
            OnLogLineReceived(text);
        }
        private async void OnLogLineReceived(string line)
        {
            Logger.LogInformation(line);
            ParseLineQueryHandler.Response result = await Mediator.Send(new ParseLineQueryHandler.Request
            {
                Line = line,
            });
            if (result != null)
            {
                if (result.EventData is ChatMessageEventModel messageInfo)
                {
                    Logger.LogInformation(messageInfo.ToString());
                }
                else if(result.EventData is SquadCreatedEventModel squadCreated)
                {
                    Logger.LogInformation(squadCreated.ToString());
                }
                else
                {
                    Logger.LogWarning("Event Received with unknown EventData type | EventName: {EventName} | EventDataType: {EventDataType}",
                        result.EventName, result.EventData?.GetType().Name ?? "null");
                }

                PluginManager.EmitEvent(result.EventName, result.EventData);
            }
        }
    }
}
