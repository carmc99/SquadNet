// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Team.Queries
{
    public static class GetTeamByNameQuery
    {
        public class Handler : IRequestHandler<Request, TeamModel>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<TeamModel>> Parser;
            private readonly IRconService RconService;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<TeamModel>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<TeamModel> Handle(Request request, CancellationToken cancellationToken)
            {
                TeamModel team = new();
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListSquads, cancellationToken);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    List<TeamModel> teams = Parser.Parse(result);
                    if (teams != null && teams.Count != 0)
                    {
                        team = teams.FirstOrDefault(p =>
                            p.Name == request.Name && p.Id == request.TeamId);
                    }
                }

                return team;
            }
        }

        public class Request : IRequest<TeamModel>
        {
            public string Name { get; set; }
            public TeamType TeamId { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();

                RuleFor(x => x.TeamId)
                    .NotEmpty();
            }
        }
    }
}