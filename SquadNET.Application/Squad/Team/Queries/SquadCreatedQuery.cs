// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Application.Squad.Team.Queries
{
    public static class SquadCreatedQuery
    {
        public class Handler : IRequestHandler<Request, SquadCreatedEventModel>
        {
            private readonly IParser<SquadCreatedEventModel> Parser;

            public Handler(IParser<SquadCreatedEventModel> parser)
            {
                Parser = parser;
            }

            public Task<SquadCreatedEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                SquadCreatedEventModel squadCreated = Parser.Parse(request.RawMessage);
                return Task.FromResult(squadCreated);
            }
        }

        public class Request : IRequest<SquadCreatedEventModel>
        {
            public string RawMessage { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.RawMessage)
                    .NotEmpty();
            }
        }
    }
}