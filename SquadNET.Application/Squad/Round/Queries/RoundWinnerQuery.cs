using FluentValidation;
using MediatR;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Round.Queries
{
    public static class RoundWinnerQuery
    {
        public class Request : IRequest<RoundWinnerEventModel>
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

        public class Handler : IRequestHandler<Request, RoundWinnerEventModel>
        {
            private readonly IParser<RoundWinnerEventModel> Parser;

            public Handler(IParser<RoundWinnerEventModel> parser)
            {
                Parser = parser;
            }

            public Task<RoundWinnerEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                RoundWinnerEventModel roundWinner = Parser.Parse(request.RawMessage);
                return Task.FromResult(roundWinner);
            }
        }
    }
}
