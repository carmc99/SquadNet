// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Team.Queries
{
    public static class GetSquadByIdQuery
    {
        public class Handler : IRequestHandler<Request, SquadInfo>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<SquadInfo>> Parser;
            private readonly IRconService RconService;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<SquadInfo>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<SquadInfo> Handle(Request request, CancellationToken cancellationToken)
            {
                SquadInfo squad = new();
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListSquads, cancellationToken);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    List<SquadInfo> squads = Parser.Parse(result);
                    if (squads != null && squads.Count != 0)
                    {
                        squad = squads.FirstOrDefault(p =>
                            p.TeamId == request.TeamId && p.Id == request.Id);
                    }
                }

                return squad;
            }
        }

        public class Request : IRequest<SquadInfo>
        {
            public int Id { get; set; }
            public TeamId TeamId { get; set; }
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