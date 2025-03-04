// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SquadNET.Application;
using SquadNET.Rcon;

using ServiceProvider serviceProvider = new ServiceCollection()
    .AddLogging(config =>
    {
        config.AddConsole();
        config.SetMinimumLevel(LogLevel.Information);
    })
    .AddSingleton<IConfiguration>(_ =>
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    })
    .AddRconServices()
    .AddSquadApplication()
    .AddSingleton<CommandHandler>()
    .BuildServiceProvider();

CommandHandler commandHandler = serviceProvider.GetRequiredService<CommandHandler>();
await commandHandler.Run();