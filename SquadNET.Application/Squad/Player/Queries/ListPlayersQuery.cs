using MediatR;
using Squadmania.Squad.Rcon.Models;
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            private readonly ICommandParser<ListPlayerModel> ListPlayersParser;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                ICommandParser<ListPlayerModel> listPlayersParser)
            {
                RconService = rconService;
                Command = command;
                ListPlayersParser = listPlayersParser;
            }

            public async Task<ListPlayerModel> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListPlayers, cancellationToken);
                ListPlayerModel players = ListPlayersParser.Parse(result);
                return players;
            }
        }
    }
}
