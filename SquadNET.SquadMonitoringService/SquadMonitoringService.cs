// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SquadNET.Application.Services;
using SquadNET.Application.Squad.ParseLine;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.LogManagement;
using System.Text;

namespace SquadNET.MonitoringService
{
    public class SquadMonitoringService : BackgroundService
    {
        private readonly IConfiguration Configuration;
        private readonly List<string> ExcludePatterns;
        private readonly bool IsFilteringEnabled;
        private readonly ILogger<SquadMonitoringService> Logger;
        private readonly ILogReaderFactory LogReaderFactory;
        private readonly ILogReader LogReaderService;
        private readonly IMediator Mediator;
        private readonly PluginManager PluginManager;
        private readonly IRconService RconService;

        public SquadMonitoringService(
            ILogger<SquadMonitoringService> logger,
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
            IsFilteringEnabled = bool.TryParse(Configuration["Filtering:IsEnabled"], out bool enabled) && enabled;
            ExcludePatterns = Configuration.GetSection("Filtering:ExcludePatterns").Get<List<string>>() ?? [];
            LogReaderService = LogReaderFactory.Create(logReaderType);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await LogReaderService.UnwatchAsync();
            await base.StopAsync(stoppingToken);
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

        private async void OnLogLineReceived(string line)
        {
            ParseLineQueryHandler.Response result = await Mediator.Send(new ParseLineQueryHandler.Request
            {
                IsFilteringEnabled = IsFilteringEnabled,
                ExcludePatterns = ExcludePatterns,
                Line = line,
            });
            if (result != null)
            {
                Logger.LogInformation("Event Received with: EventData: {EventData} | EventName: {EventName} | EventDataType: {EventDataType}",
                       result.EventData, result.EventName, result.EventData?.GetType().Name ?? "null");

                PluginManager.EmitEvent(result.EventName, result.EventData);
            }
        }

        private void OnPacketReceived(PacketInfo message)
        {
            string text = Encoding.UTF8.GetString(message.Body);
            OnLogLineReceived(text);
        }
    }
}