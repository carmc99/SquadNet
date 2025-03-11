// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Application.Squad.Admin.Queries
{
    public static class AdminBroadcastQuery
    {
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
    }
}