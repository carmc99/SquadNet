using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Player.Queries
{
    /// <summary>
    /// Query to list all players currently connected to the server.
    /// </summary>
    public static class ListPlayersQuery
    {
        public class Request : IRequest<ListPlayerModel> { }

        public class Handler : IRequestHandler<Request, ListPlayerModel>
        {
            private readonly IRconService RconService;
            private readonly Command<SquadCommand> Command;
            private readonly IParser<ListPlayerModel> Parser;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<ListPlayerModel> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<ListPlayerModel> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListPlayers, cancellationToken);
                ListPlayerModel players = Parser.Parse(result);
                return players;
            }
        }
    }
}
