// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Application.Squad.Player.Queries
{
    public static class PlayerDiedQuery
    {
        /// <summary>
        /// Handles the request by parsing the log entry into a structured event model.
        /// </summary>
        public class Handler : IRequestHandler<Request, PlayerDiedEventModel>
        {
            private readonly IParser<PlayerDiedEventModel> Parser;

            public Handler(IParser<PlayerDiedEventModel> parser)
            {
                Parser = parser;
            }

            public Task<PlayerDiedEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                PlayerDiedEventModel parsedEvent = Parser.Parse(request.RawMessage);
                return Task.FromResult(parsedEvent);
            }
        }

        public class Request : IRequest<PlayerDiedEventModel>
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