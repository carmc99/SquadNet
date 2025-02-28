using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Player.Queries
{
    /// <summary>
    /// Query to retrieve a player by their ID.
    /// </summary>
    public static class GetPlayerByIdQuery
    {
        /// <summary>
        /// Request object containing the player ID.
        /// </summary>
        public class Request : IRequest<PlayerConnectedInfo>
        {
            public CreatorOnlineIds PlayerId { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.PlayerId.SteamId)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Request, PlayerConnectedInfo>
        {
            private readonly IRconService RconService;
            private readonly Command<SquadCommand> Command;
            private readonly IParser<ListPlayerModel> Parser;


            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<ListPlayerModel> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<PlayerConnectedInfo> Handle(Request request, CancellationToken cancellationToken)
            {
                PlayerConnectedInfo player = new();
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
    }
}
