using MediatR;
using Squadmania.Squad.Rcon.Parsers;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Rcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Team.Queries
{
    /// <summary>
    /// Query to list all squads on the server.
    /// </summary>
    public static class ListSquadsQuery
    {
        public class Request : IRequest<List<TeamInfo>> { }

        public class Handler : IRequestHandler<Request, List<TeamInfo>>
        {
            private readonly IRconService RconService;
            private readonly Command<SquadCommand> Command;

            public Handler(IRconService rconService, Command<SquadCommand> command)
            {
                RconService = rconService;
                Command = command;
            }

            public async Task<List<TeamInfo>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListSquads, cancellationToken);
                return null;
                //TODO: return ListSquadsParser.Parse(result);
            }
        }
    }
}
