using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SquadNET.LogManagement;
using SquadNET.Plugins.Abstractions;
using SquadNET.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(AppContext.BaseDirectory);
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Information);
        });

        services.AddLogManagement();
        services.AddPlugins($"{AppContext.BaseDirectory}/plugins");
        services.AddHostedService<LogMonitoringService>();
    })
    .Build();

host.Run();