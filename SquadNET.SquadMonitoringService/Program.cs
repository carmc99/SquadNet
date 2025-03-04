// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using SquadNET.Application;
using SquadNET.LogManagement;
using SquadNET.MonitoringService;
using SquadNET.Plugins.Abstractions;

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
        services.AddHostedService<SquadMonitoringService>();
    })
    .Build();

await host.RunAsync();