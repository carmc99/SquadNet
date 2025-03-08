// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Application.Squad.Deployable.Queries
{
    public static class DeployableDamagedQuery
    {
        /// <summary>
        /// Handles the request by parsing the log entry into a structured event model.
        /// </summary>
        public class Handler : IRequestHandler<Request, DeployableDamagedEventModel>
        {
            private readonly IParser<DeployableDamagedEventModel> Parser;

            public Handler(IParser<DeployableDamagedEventModel> parser)
            {
                Parser = parser;
            }

            public Task<DeployableDamagedEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                DeployableDamagedEventModel parsedEvent = Parser.Parse(request.RawMessage);
                return Task.FromResult(parsedEvent);
            }
        }

        public class Request : IRequest<DeployableDamagedEventModel>
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