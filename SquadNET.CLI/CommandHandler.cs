using SquadNET.Rcon;
using Microsoft.Extensions.Logging;
using SquadNET.Core;
using SquadNET.Core.Squad.Commands;

public class CommandHandler
{
    private readonly IRconService RconService;

    public CommandHandler(IRconService rconService)
    {
        RconService = rconService;
    }

    public void Run()
    {
        Console.WriteLine("CLI RCON Iniciada. Escriba 'salir' para cerrar.");
        RconService.Connect();

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                continue;
            }

            if (input.Equals("salir", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Cerrando CLI...");
                break;
            }

            RconService.ExecuteCommandAsync(
                new SquadCommandTemplate(),
                SquadCommand.BroadcastMessage,
                input);
        }

        RconService.Disconnect();
    }
}
