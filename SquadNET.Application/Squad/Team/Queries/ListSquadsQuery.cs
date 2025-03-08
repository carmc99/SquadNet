// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
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
        public class Handler : IRequestHandler<Request, List<SquadModel>>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<SquadModel>> Parser;
            private readonly IRconService RconService;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<SquadModel>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<List<SquadModel>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListSquads, cancellationToken);
                List<SquadModel> squads = Parser.Parse(result);
                return squads;
            }
        }

        public class Request : IRequest<List<SquadModel>>
        { }
    }
}