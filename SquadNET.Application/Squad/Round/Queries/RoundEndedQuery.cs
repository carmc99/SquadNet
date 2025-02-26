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
    public static class RoundEndedQuery
    {
        public class Request : IRequest<RoundEndedEventModel>
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

        public class Handler : IRequestHandler<Request, RoundEndedEventModel>
        {
            private readonly IParser<RoundEndedEventModel> Parser;

            public Handler(IParser<RoundEndedEventModel> parser)
            {
                Parser = parser;
            }

            public Task<RoundEndedEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                RoundEndedEventModel roundEnded = Parser.Parse(request.RawMessage);
                return Task.FromResult(roundEnded);
            }
        }
    }
}
