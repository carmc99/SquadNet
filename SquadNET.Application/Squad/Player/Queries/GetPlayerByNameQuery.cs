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
    /// Query to retrieve a player by their Name.
    /// </summary>
    public static class GetPlayerByNameQuery
    {
        public class Handler : IRequestHandler<Request, PlayerConnectedInfo>
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
    }
}