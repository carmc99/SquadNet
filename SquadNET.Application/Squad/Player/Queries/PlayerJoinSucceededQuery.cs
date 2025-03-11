// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Application.Squad.Player.Queries
{
    public static class PlayerJoinSucceededQuery
    {
        public class Handler : IRequestHandler<Request, PlayerJoinSucceededEventModel>
        {
            private readonly IParser<PlayerJoinSucceededEventModel> Parser;

            public Handler(IParser<PlayerJoinSucceededEventModel> parser)
            {
                Parser = parser;
            }

            public Task<PlayerJoinSucceededEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                PlayerJoinSucceededEventModel playerJoin = Parser.Parse(request.RawMessage);
                return Task.FromResult(playerJoin);
            }
        }

        public class Request : IRequest<PlayerJoinSucceededEventModel>
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