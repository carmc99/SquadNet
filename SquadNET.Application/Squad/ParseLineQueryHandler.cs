using MediatR;
using SquadNET.Application.Squad.Chat.Commands;
using SquadNET.Application.Squad.Team.Commands;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events;
using SquadNET.Core.Squad.Models;
using System.Threading;
using System.Threading.Tasks;

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

            public Handler(IMediator mediator)
            {
                Mediator = mediator;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                ChatMessageModel chatMessage = await Mediator.Send(new ChatMessageCommand.Request
                {
                    RawMessage = request.Line,
                }, cancellationToken);

                if (chatMessage != null)
                {
                    return new Response
                    {
                        EventName = SquadEventType.CHAT_MESSAGE.ToString(),
                        EventData = chatMessage
                    };
                }

                SquadCreatedModel squadCreated = await Mediator.Send(new SquadCreatedMessageCommand.Request
                {
                    RawMessage = request.Line,
                }, cancellationToken);

                if (squadCreated != null)
                {
                    return new Response
                    {
                        EventName = SquadEventType.SQUAD_CREATED.ToString(),
                        EventData = squadCreated
                    };
                }

                return null;
            }
        }
    }
}
