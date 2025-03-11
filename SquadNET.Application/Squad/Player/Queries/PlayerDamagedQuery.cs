// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Application.Squad.Player.Queries
{
    public static class PlayerDamagedQuery
    {
        public class Handler : IRequestHandler<Request, PlayerDamagedEventModel>
        {
            private readonly IParser<PlayerDamagedEventModel> Parser;

            public Handler(IParser<PlayerDamagedEventModel> parser)
            {
                Parser = parser;
            }

            public Task<PlayerDamagedEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                PlayerDamagedEventModel playerDamaged = Parser.Parse(request.RawMessage);
                return Task.FromResult(playerDamaged);
            }
        }

        public class Request : IRequest<PlayerDamagedEventModel>
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