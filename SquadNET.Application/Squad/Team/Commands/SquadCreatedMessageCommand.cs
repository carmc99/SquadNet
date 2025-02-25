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

        public class Handler : IRequestHandler<Request, SquadCreatedModel>
        {
            private readonly IParser<SquadCreatedModel> Parser;

            public Handler(IParser<SquadCreatedModel> parser)
            {
                Parser = parser;
            }

            public Task<SquadCreatedModel> Handle(Request request, CancellationToken cancellationToken)
            {
                SquadCreatedModel squadCreated = Parser.Parse(request.RawMessage);
                return Task.FromResult(squadCreated);
            }
        }
    }
}
