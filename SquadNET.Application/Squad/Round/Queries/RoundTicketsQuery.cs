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
    public static class RoundTicketsQuery
    {
        public class Request : IRequest<RoundTicketsEventModel>
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

        public class Handler : IRequestHandler<Request, RoundTicketsEventModel>
        {
            private readonly IParser<RoundTicketsEventModel> Parser;

            public Handler(IParser<RoundTicketsEventModel> parser)
            {
                Parser = parser;
            }

            public Task<RoundTicketsEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                RoundTicketsEventModel roundTickets = Parser.Parse(request.RawMessage);
                return Task.FromResult(roundTickets);
            }
        }
    }
}
