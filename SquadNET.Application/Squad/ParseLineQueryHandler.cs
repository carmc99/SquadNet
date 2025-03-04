// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using MediatR;
using Microsoft.Extensions.Logging;
using SquadNET.Application.Squad.Admin.Queries;
using SquadNET.Application.Squad.Chat.Commands;
using SquadNET.Application.Squad.Player.Queries;
using SquadNET.Application.Squad.Round.Queries;
using SquadNET.Application.Squad.Server.Queries;
using SquadNET.Application.Squad.Team.Queries;
using SquadNET.Core.Squad.Events;
using System.Text.RegularExpressions;

namespace SquadNET.Application.Squad.ParseLine
{
    public static class ParseLineQueryHandler
    {
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ILogger Logger;
            private readonly IMediator Mediator;
            private readonly Dictionary<SquadEventType, Func<string, CancellationToken, Task<ISquadEventData>>> Parsers;

            public Handler(IMediator mediator, ILogger<Handler> logger)
            {
                Logger = logger;
                Mediator = mediator;

                Parsers = new Dictionary<SquadEventType, Func<string, CancellationToken, Task<ISquadEventData>>>
                {
                    { SquadEventType.ChatMessage, (line, ct) => ParseChatMessage(line, ct) },
                    { SquadEventType.SquadCreated, (line, ct) => ParseSquadCreated(line, ct) },
                    { SquadEventType.GameEnded, (line, ct) => ParseRoundEnded(line, ct) },
                    { SquadEventType.RoundTickets, (line, ct) => ParseRoundTickets(line, ct) },
                    { SquadEventType.RoundWinner, (line, ct) => ParseRoundWinner(line, ct) },
                    { SquadEventType.ServerTickRateUpdated, (line, ct) => ParseServerTickRate(line, ct) },
                    { SquadEventType.PlayerDamaged, (line, ct) => ParsePlayerDamaged(line, ct) },
                    { SquadEventType.PlayerConnected, (line, ct) => ParsePlayerJoinSucceeded(line, ct) },
                    { SquadEventType.PlayerPossessed, (line, ct) => ParsePlayerPossess(line, ct) },
                    { SquadEventType.PlayerUnpossessed, (line, ct) => ParsePlayerUnposess(line, ct)},
                    { SquadEventType.PlayerRevived, (line, ct) => ParsePlayerRevived(line, ct) },
                    { SquadEventType.AdminBroadcast, (line, ct) => ParseAdminBroadcast(line, ct) }
                    //TODO:
                    // deployable-damaged
                    // player-died
                    // player-disconnected
                    // player-join-succeeded
                    // player-wounded
                };
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.IsFilteringEnabled && ShouldExclude(request.Line, request.ExcludePatterns))
                {
                    return null;
                }

                Logger.LogInformation("{Line}", request.Line);

                foreach (KeyValuePair<SquadEventType, Func<string, CancellationToken, Task<ISquadEventData>>> entry in Parsers)
                {
                    ISquadEventData result = await entry.Value(request.Line, cancellationToken);
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

            private async Task<ISquadEventData> ParseAdminBroadcast(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new AdminBroadcastQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<ISquadEventData> ParseChatMessage(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new ChatMessageQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<ISquadEventData> ParsePlayerDamaged(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new PlayerDamagedQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<ISquadEventData> ParsePlayerJoinSucceeded(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new PlayerJoinSucceededQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<ISquadEventData> ParsePlayerPossess(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new PlayerPossessQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<ISquadEventData> ParsePlayerRevived(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new PlayerRevivedQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<ISquadEventData> ParsePlayerUnposess(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new PlayerUnPossessQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<ISquadEventData> ParseRoundEnded(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new RoundEndedQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<ISquadEventData> ParseRoundTickets(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new RoundTicketsQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<ISquadEventData> ParseRoundWinner(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new RoundWinnerQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<ISquadEventData> ParseServerTickRate(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new ServerTickRateQuery.Request { RawMessage = line }, cancellationToken);
            }

            private async Task<ISquadEventData> ParseSquadCreated(string line, CancellationToken cancellationToken)
            {
                return await Mediator.Send(new SquadCreatedQuery.Request { RawMessage = line }, cancellationToken);
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
        }

        public class Request : IRequest<Response>
        {
            public List<string> ExcludePatterns { get; set; } = [];
            public bool IsFilteringEnabled { get; set; } = false;
            public string Line { get; set; }
        }

        public class Response
        {
            public ISquadEventData EventData { get; set; }
            public string EventName { get; set; }
        }
    }
}