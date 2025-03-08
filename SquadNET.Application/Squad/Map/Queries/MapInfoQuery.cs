// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Map.Queries
{
    public static class MapInfoQuery
    {
        public class Handler : IRequestHandler<Request, MapModel>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<CurrentMapModel> CurrentMapParser;
            private readonly IParser<NextMapModel> NextMapParser;
            private readonly IRconService RconService;

            public Handler(
                IRconService rconService,
                IParser<CurrentMapModel> currentMapParser,
                IParser<NextMapModel> nextMapParser,
                Command<SquadCommand> command)
            {
                RconService = rconService;
                CurrentMapParser = currentMapParser;
                NextMapParser = nextMapParser;
                Command = command;
            }

            public async Task<MapModel> Handle(Request request, CancellationToken cancellationToken)
            {
                string currentMapResponse = await RconService.ExecuteCommandAsync(Command, SquadCommand.ShowCurrentMap);
                string nextMapResponse = await RconService.ExecuteCommandAsync(Command, SquadCommand.ShowNextMap);

                MapModel mapInfo = new()
                {
                    CurrentMap = CurrentMapParser.Parse(currentMapResponse),
                    NextMap = NextMapParser.Parse(nextMapResponse)
                };

                return mapInfo;
            }
        }

        public class Request : IRequest<MapModel>
        { }
    }
}