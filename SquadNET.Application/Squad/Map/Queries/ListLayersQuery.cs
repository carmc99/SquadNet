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

namespace SquadNET.Application.Squad.Map.Queries
{
    /// <summary>
    /// Query to list all available layers on the server.
    /// </summary>
    public static class ListLayersQuery
    {
        public class Handler : IRequestHandler<Request, List<LayerModel>>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<LayerModel>> Parser;
            private readonly IRconService RconService;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<LayerModel>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<List<LayerModel>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListLayers, cancellationToken);
                List<LayerModel> layers = Parser.Parse(result);
                return layers;
            }
        }

        public class Request : IRequest<List<LayerModel>>
        { }
    }
}