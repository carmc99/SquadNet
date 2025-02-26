using FluentValidation;
using MediatR;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Server.Queries
{
    public static class ServerTickRateQuery
    {
        public class Request : IRequest<ServerTickRateEventModel>
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

        public class Handler : IRequestHandler<Request, ServerTickRateEventModel>
        {
            private readonly IParser<ServerTickRateEventModel> Parser;

            public Handler(IParser<ServerTickRateEventModel> parser)
            {
                Parser = parser;
            }

            public Task<ServerTickRateEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                ServerTickRateEventModel tickRate = Parser.Parse(request.RawMessage);
                return Task.FromResult(tickRate);
            }
        }
    }
}
