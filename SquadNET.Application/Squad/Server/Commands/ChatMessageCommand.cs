using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Rcon;
using System.Threading;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Chat.Commands
{
    public static class ChatMessageCommand
    {
        public class Request : IRequest<ChatMessageEventModel>
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

        public class Handler : IRequestHandler<Request, ChatMessageEventModel>
        {
            private readonly IParser<ChatMessageInfo> Parser;

            public Handler(IParser<ChatMessageInfo> parser)
            {
                Parser = parser;
            }

            public Task<ChatMessageEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                ChatMessageEventModel model = null;
                ChatMessageInfo chatMessage = Parser.Parse(request.RawMessage);
                if (chatMessage != null)
                {
                    model = ChatMessageEventModel.FromEntity(chatMessage);
                }
                return Task.FromResult(model);
            }
        }
    }
}
