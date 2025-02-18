using SquadNET.Rcon;
using Microsoft.Extensions.Logging;

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

            RconService.ExecuteCommandAsync(RconCommand.BroadcastMessage, input);
        }

        RconService.Disconnect();
    }
}
