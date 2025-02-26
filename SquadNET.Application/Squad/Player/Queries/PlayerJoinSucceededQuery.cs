using FluentValidation;
using MediatR;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Player.Queries
{
    public static class PlayerJoinSucceededQuery
    {
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
    }
}
