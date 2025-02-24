using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Map.Queries
{
    /// <summary>
    /// Query to list all available layers on the server.
    /// </summary>
    public static class ListLayersQuery
    {
        public class Request : IRequest<List<LayerInfo>> { }

        public class Handler : IRequestHandler<Request, List<LayerInfo>>
        {
            private readonly IRconService RconService;
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<LayerInfo>> Parser;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<LayerInfo>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<List<LayerInfo>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListLayers, cancellationToken);
                List<LayerInfo> layers = Parser.Parse(result);
                return layers;
            }
        }
    }
}
