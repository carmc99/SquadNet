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

namespace SquadNET.Application.Squad.Admin.Queries
{
    public static class AdminBroadcastQuery
    {
        public class Request : IRequest<AdminBroadcastEventModel>
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

        public class Handler : IRequestHandler<Request, AdminBroadcastEventModel>
        {
            private readonly IParser<AdminBroadcastEventModel> Parser;

            public Handler(IParser<AdminBroadcastEventModel> parser)
            {
                Parser = parser;
            }

            public Task<AdminBroadcastEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                AdminBroadcastEventModel adminBroadcast = Parser.Parse(request.RawMessage);
                return Task.FromResult(adminBroadcast);
            }
        }
    }
}
