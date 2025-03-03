using FluentValidation;
using MediatR;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using SquadNET.Core;
using SquadNET.Rcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Player.Queries
{
    /// <summary>
    /// Query to retrieve a player by their Name.
    /// </summary>
    public static class GetPlayerByNameQuery
    {
        /// <summary>
        /// Request containing the player Name.
        /// </summary>
        public class Request : IRequest<PlayerConnectedInfo>
        {
            public string Name { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
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
                        player = players.ActivePlayers.FirstOrDefault(p => p.Name == request.Name);
                    }
                }

                return player;
            }
        }
    }
}
