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
        public class Request : IRequest<List<TeamInfo>> { }

        public class Handler : IRequestHandler<Request, List<TeamInfo>>
        {
            private readonly IRconService RconService;
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<TeamInfo>> Parser;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<TeamInfo>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<List<TeamInfo>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListSquads, cancellationToken);
                List<TeamInfo> squads = Parser.Parse(result);
                return squads;
            }
        }
    }
}
