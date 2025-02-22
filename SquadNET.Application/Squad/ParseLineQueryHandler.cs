using MediatR;
using SquadNET.Application.Squad.Chat.Queries;
using SquadNET.Core.Squad.Entities;
using SquadNET.Plugins.Abstractions.Squad;
using System.Threading;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.ParseLine
{
    public class ParsedEventResult
    {
        public string EventName { get; set; }
        public object EventData { get; set; }
    }

    public class ParseLineQuery : IRequest<ParsedEventResult>
    {
        public string Line { get; }
        public ParseLineQuery(string line)
        {
            Line = line;
        }
    }

    public class ParseLineQueryHandler : IRequestHandler<ParseLineQuery, ParsedEventResult>
    {
        private readonly IMediator Mediator;

        public ParseLineQueryHandler(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task<ParsedEventResult> Handle(ParseLineQuery request, CancellationToken cancellationToken)
        {
            ChatMessageInfo chatMessage = await Mediator.Send(new ChatMessageQuery.Request(request.Line), cancellationToken);
            if (chatMessage != null)
            {
                return new ParsedEventResult
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
