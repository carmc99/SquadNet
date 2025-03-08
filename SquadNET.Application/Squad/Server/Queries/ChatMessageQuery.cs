// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Application.Squad.Chat.Commands
{
    public static class ChatMessageQuery
    {
        public class Handler : IRequestHandler<Request, ChatMessageEventModel>
        {
            private readonly IParser<ChatMessageEventModel> Parser;

            public Handler(IParser<ChatMessageEventModel> parser)
            {
                Parser = parser;
            }

            public Task<ChatMessageEventModel> Handle(Request request, CancellationToken cancellationToken)
            {
                ChatMessageEventModel chatMessage = Parser.Parse(request.RawMessage);
                return Task.FromResult(chatMessage);
            }
        }

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
    }
}