using MediatR;
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
        public class Request : IRequest<List<SquadInfo>> { }

        public class Handler : IRequestHandler<Request, List<SquadInfo>>
        {
            private readonly IRconService RconService;
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<SquadInfo>> Parser;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<SquadInfo>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<List<SquadInfo>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListSquads, cancellationToken);
                List<SquadInfo> squads = Parser.Parse(result);
                return squads;
            }
        }
    }
}
