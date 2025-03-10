// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Application.Squad.Player.Queries
{
    public static class PlayerWoundedQuery
    {
        /// <summary>
        /// Handles the request by parsing the log entry into a structured event model.
        /// </summary>
        public class Handler : IRequestHandler<Request, PlayerWoundedEventModel>
        {
            //private readonly IParser<PlayerWoundedEventModel> Parser;

            //public Handler(IParser<PlayerWoundedEventModel> parser)
            //{
            //    Parser = parser;
            //}

            public Task<PlayerWoundedEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                //TODO: Complete
                throw new NotImplementedException();
                //PlayerWoundedEventModel parsedEvent = Parser.Parse(request.RawMessage);
                //return Task.FromResult(parsedEvent);
            }
        }

        public class Request : IRequest<PlayerWoundedEventModel>
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