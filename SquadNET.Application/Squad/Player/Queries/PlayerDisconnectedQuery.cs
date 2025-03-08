// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Application.Squad.Player.Queries
{
    public static class PlayerDisconnectedQuery
    {
        /// <summary>
        /// Handles the request by parsing the log entry into a structured event model.
        /// </summary>
        public class Handler : IRequestHandler<Request, PlayerDisconnectedEventModel>
        {
            private readonly IParser<PlayerDisconnectedEventModel> Parser;

            public Handler(IParser<PlayerDisconnectedEventModel> parser)
            {
                Parser = parser;
            }

            public Task<PlayerDisconnectedEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                PlayerDisconnectedEventModel parsedEvent = Parser.Parse(request.RawMessage);
                return Task.FromResult(parsedEvent);
            }
        }

        public class Request : IRequest<PlayerDisconnectedEventModel>
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