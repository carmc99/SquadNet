// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using MediatR;
using SquadNET.Application.Squad.Server.Queries;
using SquadNET.Core;

public class CommandHandler
{
    private readonly IMediator Mediator;
    private readonly IRconService RconService;

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

            var response = await Mediator.Send(new ServerInformationQuery.Request());

            //BroadcastMessageCommand.Request request = new() { Message = input };
            //string response = await Mediator.Send(request);

            //Console.WriteLine(response);
        }
    }
}