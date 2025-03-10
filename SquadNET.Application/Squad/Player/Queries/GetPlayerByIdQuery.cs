// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Player.Queries
{
    /// <summary>
    /// Query to retrieve a player by their ID.
    /// </summary>
    public static class GetPlayerByIdQuery
    {
        public class Handler : IRequestHandler<Request, PlayerConnectedModel>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<ListPlayerModel> Parser;
            private readonly IRconService RconService;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<ListPlayerModel> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<PlayerConnectedModel> Handle(Request request, CancellationToken cancellationToken)
            {
                PlayerConnectedModel player = new();
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListPlayers, cancellationToken);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    ListPlayerModel players = Parser.Parse(result);
                    if (players != null && players.ActivePlayers.Count != 0)
                    {
                        player = players.ActivePlayers.FirstOrDefault(p => p.CreatorIds == request.PlayerId);
                    }
                }

                return player;
            }
        }

        /// <summary>
        /// Request object containing the player ID.
        /// </summary>
        public class Request : IRequest<PlayerConnectedModel>
        {
            public CreatorOnlineModel PlayerId { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.PlayerId.SteamId)
                    .NotEmpty();
            }
        }
    }
}