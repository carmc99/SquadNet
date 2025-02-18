using SquadNET.Rcon;
using Microsoft.Extensions.Logging;
using SquadNET.Core;
using SquadNET.Core.Squad.Commands;
using MediatR;
using SquadNET.Application.Squad.Admin.Commands;

public class CommandHandler
{
    private readonly IRconService RconService;
    private readonly IMediator Mediator;
    public CommandHandler(IRconService rconService, IMediator mediator)
    {
        RconService = rconService;
        Mediator = mediator;
    }

    public async Task Run()
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

            BroadcastMessageCommand.Request request = new() { Message = input };
            string response = await Mediator.Send(request);

            Console.WriteLine(response);    
        }

        RconService.Disconnect();
    }
}
