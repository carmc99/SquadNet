using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    .AddSingleton<CommandHandler>()
    .BuildServiceProvider();

CommandHandler commandHandler = serviceProvider.GetRequiredService<CommandHandler>();
commandHandler.Run();