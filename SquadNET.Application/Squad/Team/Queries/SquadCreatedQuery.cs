using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Rcon;
using System.Threading;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Team.Queries
{
    public static class SquadCreatedQuery
    {
        public class Request : IRequest<SquadCreatedEventModel>
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
        public class Handler : IRequestHandler<Request, SquadCreatedEventModel>
        {
            private readonly IParser<SquadCreatedInfo> Parser;

            public Handler(IParser<SquadCreatedInfo> parser)
            {
                Parser = parser;
            }

            public Task<SquadCreatedEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                SquadCreatedEventModel model = null;
                SquadCreatedInfo squadCreated = Parser.Parse(request.RawMessage);
                if (squadCreated != null)
                {
                    model = SquadCreatedEventModel.FromEntity(squadCreated);
                }
                return Task.FromResult(model);
            }
        }
    }
}
