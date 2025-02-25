using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;
using System.Threading;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Chat.Commands
{
    public static class ChatMessageCommand
    {
        public class Request : IRequest<ChatMessageModel>
        {
            public string ChatMessageRaw { get; set; }
        }

        public class Handler : IRequestHandler<Request, ChatMessageModel>
        {
            private readonly IParser<ChatMessageInfo> Parser;

            public Handler(IParser<ChatMessageInfo> parser)
            {
                Parser = parser;
            }

            public Task<ChatMessageModel> Handle(Request request, CancellationToken cancellationToken)
            {

                ChatMessageInfo chatMessage = Parser.Parse(request.ChatMessageRaw);
                return Task.FromResult(ChatMessageModel.FromEntity(chatMessage));
            }
        }
    }
}
