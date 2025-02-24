using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Rcon;
using System.Threading;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Map.Queries
{
    public static class MapInfoQuery
    {
        public class Request : IRequest<MapInfo> { }

        public class Handler : IRequestHandler<Request, MapInfo>
        {
            private readonly IRconService RconService;
            private readonly IParser<CurrentMapInfo> CurrentMapParser;
            private readonly IParser<NextMapInfo> NextMapParser;
            private readonly Command<SquadCommand> Command;

            public Handler(
                IRconService rconService,
                IParser<CurrentMapInfo> currentMapParser,
                IParser<NextMapInfo> nextMapParser,
                Command<SquadCommand> command)
            {
                RconService = rconService;
                CurrentMapParser = currentMapParser;
                NextMapParser = nextMapParser;
                Command = command;
            }

            public async Task<MapInfo> Handle(Request request, CancellationToken cancellationToken)
            {
                string currentMapResponse = await RconService.ExecuteCommandAsync(Command, SquadCommand.ShowCurrentMap);
                string nextMapResponse = await RconService.ExecuteCommandAsync(Command, SquadCommand.ShowNextMap);

                MapInfo mapInfo = new()
                {
                    CurrentMap = CurrentMapParser.Parse(currentMapResponse),
                    NextMap = NextMapParser.Parse(nextMapResponse)
                };

                return mapInfo;
            }
        }
    }
}
