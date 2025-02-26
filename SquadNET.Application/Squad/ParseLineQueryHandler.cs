using MediatR;
using SquadNET.Core.Squad.Models;
using SquadNET.Core.Squad.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SquadNET.Application.Squad.Chat.Commands;
using SquadNET.Application.Squad.Team.Commands;

namespace SquadNET.Application.Squad.ParseLine
{
    public static class ParseLineQueryHandler
    {
        public class Request : IRequest<Response>
        {
            public string Line { get; set; }
        }

        public class Response
        {
            public string EventName { get; set; }
            public IEventData EventData { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMediator Mediator;
            private readonly Dictionary<SquadEventType,
                Func<string, CancellationToken, Task<IEventData>>> Parsers;

            public Handler(IMediator mediator)
            {
                Mediator = mediator;

                Parsers = new Dictionary<SquadEventType, Func<string, CancellationToken, Task<IEventData>>>
                {
                    { SquadEventType.ChatMessage, (line, ct) => ParseChatMessage(line, ct) },
                    { SquadEventType.SquadCreated, (line, ct) => ParseSquadCreated(line, ct) }
                };
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                foreach (KeyValuePair<SquadEventType, Func<string, CancellationToken, Task<IEventData>>> entry in Parsers)
                {
                    IEventData result = await entry.Value(request.Line, cancellationToken);
                    if (result != null)
                    {
                        return new Response
                        {
                            EventName = entry.Key.ToString(),
                            EventData = result
                        };
                    }
                }

                return null;
            }

            private async Task<IEventData> ParseChatMessage(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new ChatMessageCommand.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<IEventData> ParseSquadCreated(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new SquadCreatedMessageCommand.Request { RawMessage = line }, cancellationToken);
            }
        }
    }
}
