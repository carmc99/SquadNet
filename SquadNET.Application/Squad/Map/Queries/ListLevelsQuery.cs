// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
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
        public class Handler : IRequestHandler<Request, List<LevelModel>>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<LevelModel>> Parser;
            private readonly IRconService RconService;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<LevelModel>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<List<LevelModel>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListLevels, cancellationToken);
                List<LevelModel> levels = Parser.Parse(result);
                return levels;
            }
        }

        public class Request : IRequest<List<LevelModel>>
        { }
    }
}