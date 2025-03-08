// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Team.Queries
{
    /// <summary>
    /// Query to list all teams on the server.
    /// </summary>
    public static class ListTeamsQuery
    {
        public class Handler : IRequestHandler<Request, List<TeamModel>>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<TeamModel>> Parser;
            private readonly IRconService RconService;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<TeamModel>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<List<TeamModel>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListSquads, cancellationToken);
                List<TeamModel> squads = Parser.Parse(result);
                return squads;
            }
        }

        public class Request : IRequest<List<TeamModel>>
        { }
    }
}