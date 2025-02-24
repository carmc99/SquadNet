using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Rcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Map.Queries
{
    /// <summary>
    /// Query to list all available levels on the server.
    /// </summary>
    public static class ListLevelsQuery
    {
        public class Request : IRequest<List<LevelInfo>> { }

        public class Handler : IRequestHandler<Request, List<LevelInfo>>
        {
            private readonly IRconService RconService;
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<LevelInfo>> Parser;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<LevelInfo>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<List<LevelInfo>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListLevels, cancellationToken);
                List<LevelInfo> levels = Parser.Parse(result);
                return levels;
            }
        }
    }
}
