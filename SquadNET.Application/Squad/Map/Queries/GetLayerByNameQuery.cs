// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Map.Queries
{
    /// <summary>
    /// Query to retrieve a layer by their Name.
    /// </summary>
    public static class GetLayerByNameQuery
    {
        public class Handler : IRequestHandler<Request, LayerModel>
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

            public async Task<LayerModel> Handle(Request request, CancellationToken cancellationToken)
            {
                LayerModel layer = new();
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListLayers, cancellationToken);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    List<LayerModel> layers = Parser.Parse(result);
                    if (layers != null && layers.Count != 0)
                    {
                        layer = layers.FirstOrDefault(l => l.Name == request.Name);
                    }
                }

                return layer;
            }
        }

        /// <summary>
        /// Request containing the layer Name.
        /// </summary>
        public class Request : IRequest<LayerModel>
        {
            public string Name { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();
            }
        }
    }
}