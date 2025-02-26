using FluentValidation;
using MediatR;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Player.Queries
{
    public static class PlayerRevivedQuery
    {
        public class Request : IRequest<PlayerRevivedEventModel>
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

        public class Handler : IRequestHandler<Request, PlayerRevivedEventModel>
        {
            private readonly IParser<PlayerRevivedEventModel> Parser;

            public Handler(IParser<PlayerRevivedEventModel> parser)
            {
                Parser = parser;
            }

            public Task<PlayerRevivedEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                PlayerRevivedEventModel playerRevived = Parser.Parse(request.RawMessage);
                return Task.FromResult(playerRevived);
            }
        }
    }
}
