// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Team.Queries
{
    public static class GetSquadByIdQuery
    {
        public class Handler : IRequestHandler<Request, SquadModel>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<SquadModel>> Parser;
            private readonly IRconService RconService;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<SquadModel>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<SquadModel> Handle(Request request, CancellationToken cancellationToken)
            {
                SquadModel squad = new();
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListSquads, cancellationToken);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    List<SquadModel> squads = Parser.Parse(result);
                    if (squads != null && squads.Count != 0)
                    {
                        squad = squads.FirstOrDefault(p =>
                            p.TeamId == request.TeamId && p.Id == request.Id);
                    }
                }

                return squad;
            }
        }

        public class Request : IRequest<SquadModel>
        {
            public int Id { get; set; }
            public TeamType TeamId { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();

                RuleFor(x => x.TeamId)
                    .NotEmpty();
            }
        }
    }
}