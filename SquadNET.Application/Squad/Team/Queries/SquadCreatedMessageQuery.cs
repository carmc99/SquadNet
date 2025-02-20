using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Rcon;
using System.Threading;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Queries
{
    public static class SquadCreatedMessageQuery
    {
        public class Request : IRequest<SquadCreatedInfo>
        {
            public string RawMessage { get; }

            public Request(string rawMessage)
            {
                RawMessage = rawMessage;
            }
        }

        public class Handler : IRequestHandler<Request, SquadCreatedInfo>
        {
            private readonly ICommandParser<SquadCreatedInfo> Parser;

            public Handler(ICommandParser<SquadCreatedInfo> parser)
            {
                Parser = parser;
            }

            public Task<SquadCreatedInfo> Handle(Request request, CancellationToken cancellationToken)
            {
                SquadCreatedInfo squadInfo = Parser.Parse(request.RawMessage);
                return Task.FromResult(squadInfo);
            }
        }
    }
}
