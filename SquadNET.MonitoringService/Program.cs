using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using SquadNET.Application;
using SquadNET.LogManagement;
using SquadNET.Plugins.Abstractions;
using SquadNET.MonitoringService;


Logger logger = new LoggerConfiguration()
    .WriteTo.Console(new CustomConsoleFormatter())
    .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(AppContext.BaseDirectory);
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .UseSerilog(logger)
    .ConfigureServices((context, services) =>
    {
        services.AddLogManagement();
        services.AddSquadApplication();
        services.AddPlugins(Path.Combine(AppContext.BaseDirectory, "plugins"));
        services.AddHostedService<MonitoringService>();
    })
    .Build();

host.Run();