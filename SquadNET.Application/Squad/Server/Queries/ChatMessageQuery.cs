using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Rcon;
using System.Threading;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Chat.Queries
{
    public static class ChatMessageQuery
    {
        public class Request : IRequest<ChatMessageInfo>
        {
            public string ChatMessageRaw { get; }

            public Request(string chatMessageRaw)
            {
                ChatMessageRaw = chatMessageRaw;
            }
        }

        public class Handler : IRequestHandler<Request, ChatMessageInfo?>
        {
            private readonly ICommandParser<ChatMessageInfo> Parser;

            public Handler(ICommandParser<ChatMessageInfo> parser)
            {
                Parser = parser;
            }

            public Task<ChatMessageInfo?> Handle(Request request, CancellationToken cancellationToken)
            {
                ChatMessageInfo chatMessage = Parser.Parse(request.ChatMessageRaw);
                return Task.FromResult(chatMessage);
            }
        }
    }
}
