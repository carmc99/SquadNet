// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Application.Squad.Player.Queries
{
    public static class PlayerUnPossessQuery
    {
        public class Handler : IRequestHandler<Request, PlayerUnPossessEventModel>
        {
            private readonly IParser<PlayerUnPossessEventModel> Parser;

            public Handler(IParser<PlayerUnPossessEventModel> parser)
            {
                Parser = parser;
            }

            public Task<PlayerUnPossessEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                PlayerUnPossessEventModel playerUnPossess = Parser.Parse(request.RawMessage);
                return Task.FromResult(playerUnPossess);
            }
        }

        public class Request : IRequest<PlayerUnPossessEventModel>
        {
            public string RawMessage { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.RawMessage).NotEmpty();
            }
        }
    }
}