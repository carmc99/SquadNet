using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;
using System.Threading;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Chat.Queries
{
    public static class ChatMessageQuery
    {
        public class Request : IRequest<ChatMessageInfoModel>
        {
            public string ChatMessageRaw { get; set; }
        }

        public class Handler : IRequestHandler<Request, ChatMessageInfoModel>
        {
            private readonly IParser<ChatMessageInfo> Parser;

            public Handler(IParser<ChatMessageInfo> parser)
            {
                Parser = parser;
            }

            public Task<ChatMessageInfoModel> Handle(Request request, CancellationToken cancellationToken)
            {

                ChatMessageInfo chatMessage = Parser.Parse(request.ChatMessageRaw);
                return Task.FromResult(ChatMessageInfoModel.FromEntity(chatMessage));
            }
        }
    }
}
