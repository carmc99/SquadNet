// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Player.Queries
{
    public static class PlayerDisconnectedQuery
    {
        /// <summary>
        /// Handles the request by parsing the log entry into a structured event model.
        /// </summary>
        public class Handler : IRequestHandler<Request, PlayerDisconnectedModel>
        {
            private readonly IParser<PlayerDisconnectedModel> Parser;

            public Handler(IParser<PlayerDisconnectedModel> parser)
            {
                Parser = parser;
            }

            public Task<PlayerDisconnectedModel> Handle(Request request, CancellationToken cancellationToken)
            {
                PlayerDisconnectedModel parsedEvent = Parser.Parse(request.RawMessage);
                return Task.FromResult(parsedEvent);
            }
        }

        public class Request : IRequest<PlayerDisconnectedModel>
        {
            public string RawMessage { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.RawMessage)
                    .NotEmpty();
            }
        }
    }
}