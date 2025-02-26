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
    public static class PlayerDamagedQuery
    {
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
    }
}
