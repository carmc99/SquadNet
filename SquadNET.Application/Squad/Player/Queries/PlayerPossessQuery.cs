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
    public static class PlayerPossessQuery
    {
        public class Request : IRequest<PlayerPossessEventModel>
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

        public class Handler : IRequestHandler<Request, PlayerPossessEventModel>
        {
            private readonly IParser<PlayerPossessEventModel> Parser;

            public Handler(IParser<PlayerPossessEventModel> parser)
            {
                Parser = parser;
            }

            public Task<PlayerPossessEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                PlayerPossessEventModel playerPossess = Parser.Parse(request.RawMessage);
                return Task.FromResult(playerPossess);
            }
        }
    }
}
