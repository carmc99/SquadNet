using MediatR;
using SquadNET.Core.Squad.Models;
using SquadNET.Core.Squad.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SquadNET.Application.Squad.Server.Queries;
using SquadNET.Application.Squad.Team.Queries;
using SquadNET.Application.Squad.Chat.Commands;
using SquadNET.Application.Squad.Admin.Queries;
using SquadNET.Application.Squad.Player.Queries;
using SquadNET.Application.Squad.Round.Queries;
using System.Text.RegularExpressions;

namespace SquadNET.Application.Squad.ParseLine
{
    public static class ParseLineQueryHandler
    {
        public class Request : IRequest<Response>
        {
            public string Line { get; set; }
            public bool IsFilteringEnabled { get; set; } = false;
            public List<string> ExcludePatterns { get; set; } = [];
        }

        public class Response
        {
            public string EventName { get; set; }
            public IEventData EventData { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMediator Mediator;
            private readonly Dictionary<SquadEventType, Func<string, CancellationToken, Task<IEventData>>> Parsers;

            public Handler(IMediator mediator)
            {
                Mediator = mediator;

                Parsers = new Dictionary<SquadEventType, Func<string, CancellationToken, Task<IEventData>>>
                {
                    { SquadEventType.ChatMessage, (line, ct) => ParseChatMessage(line, ct) },
                    { SquadEventType.SquadCreated, (line, ct) => ParseSquadCreated(line, ct) },
                    // TODO: Revisar
                    //{ SquadEventType.RoundEnded, (line, ct) => ParseRoundEnded(line, ct) },
                    //{ SquadEventType.RoundTickets, (line, ct) => ParseRoundTickets(line, ct) },
                    //{ SquadEventType.RoundWinner, (line, ct) => ParseRoundWinner(line, ct) },
                    { SquadEventType.ServerTickRateUpdated, (line, ct) => ParseServerTickRate(line, ct) },
                    { SquadEventType.PlayerDamaged, (line, ct) => ParsePlayerDamaged(line, ct) },
                    { SquadEventType.PlayerConnected, (line, ct) => ParsePlayerJoinSucceeded(line, ct) },
                    { SquadEventType.PlayerPossessed, (line, ct) => ParsePlayerPossess(line, ct) },
                    { SquadEventType.PlayerRevived, (line, ct) => ParsePlayerRevived(line, ct) },
                    { SquadEventType.AdminBroadcast, (line, ct) => ParseAdminBroadcast(line, ct) }
                };
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.IsFilteringEnabled && ShouldExclude(request.Line, request.ExcludePatterns))
                {
                    return null;
                }

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

            private bool ShouldExclude(string line, List<string> excludePatterns)
            {
                foreach (string pattern in excludePatterns)
                {
                    if (Regex.IsMatch(line, pattern, RegexOptions.IgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            private async Task<IEventData> ParseChatMessage(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new ChatMessageQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<IEventData> ParseSquadCreated(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new SquadCreatedQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<IEventData> ParseRoundEnded(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new RoundEndedQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<IEventData> ParseRoundTickets(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new RoundTicketsQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<IEventData> ParseRoundWinner(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new RoundWinnerQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<IEventData> ParseServerTickRate(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new ServerTickRateQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<IEventData> ParsePlayerDamaged(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new PlayerDamagedQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<IEventData> ParsePlayerJoinSucceeded(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new PlayerJoinSucceededQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<IEventData> ParsePlayerPossess(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new PlayerPossessQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<IEventData> ParsePlayerRevived(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new PlayerRevivedQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<IEventData> ParseAdminBroadcast(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new AdminBroadcastQuery.Request { RawMessage = line }, cancellationToken);
            }
        }
    }
}
