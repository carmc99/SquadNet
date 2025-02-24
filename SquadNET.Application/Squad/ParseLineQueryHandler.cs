using MediatR;
using SquadNET.Application.Squad.Chat.Queries;
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
                ChatMessageInfoModel chatMessage = await Mediator.Send(new ChatMessageQuery.Request
                {
                    ChatMessageRaw = request.Line,
                }, cancellationToken);

                if (chatMessage != null)
                {
                    return new Response
                    {
                        EventName = LogEventType.CHAT_MESSAGE.ToString(),
                        EventData = chatMessage
                    };
                }

                // 2) Intentamos parsear "teamkill" (si tuvieras un TeamKillQuery)
                /*
                var teamKill = await Mediator.Send(new TeamKillQuery.Request(request.Line), cancellationToken);
                if (teamKill != null)
                {
                    return new ParsedEventResult
                    {
                        EventName = LogEvents.TEAMKILL,
                        EventData = teamKill
                    };
                }
                */

                // 3) Otros eventos que quieras

                // Si no coinciden ninguno, retornamos null
                return null;
            }
        }
    }
}
