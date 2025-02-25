using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;
using System.Threading;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Team.Commands
{
    public static class SquadCreatedMessageCommand
    {
        public class Request : IRequest<SquadCreatedModel>
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
        public class Handler : IRequestHandler<Request, SquadCreatedModel>
        {
            private readonly IParser<SquadCreatedInfo> Parser;

            public Handler(IParser<SquadCreatedInfo> parser)
            {
                Parser = parser;
            }

            public Task<SquadCreatedModel> Handle(Request request, CancellationToken cancellationToken)
            {
                SquadCreatedModel model = null;
                SquadCreatedInfo squadCreated = Parser.Parse(request.RawMessage);
                if (squadCreated != null)
                {
                    model = SquadCreatedModel.FromEntity(squadCreated);
                }
                return Task.FromResult(model);
            }
        }
    }
}
