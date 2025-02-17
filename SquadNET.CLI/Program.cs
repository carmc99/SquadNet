using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SquadNET.Rcon;
using System;
using System.IO;
using System.Threading.Tasks;

Console.WriteLine("Iniciando aplicación...");

// Configuración de servicios en .NET 8 (Top-Level Statements)
using var serviceProvider = new ServiceCollection()
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
    .BuildServiceProvider();

// Obtener el servicio RCON
var rconService = serviceProvider.GetRequiredService<IRconService>();

try
{
    await rconService.ConnectAsync();
    string response = await rconService.ExecuteCommandAsync(RconCommand.BroadcastMessage,
        "Hola mundo");
    Console.WriteLine($"Respuesta del servidor: {response}");
    await rconService.DisconnectAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.WriteLine("Aplicación finalizada.");
