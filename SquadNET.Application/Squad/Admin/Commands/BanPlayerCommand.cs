using FluentValidation;
using MediatR;
using SquadNET.Core;
using SquadNET.Rcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Admin.Commands
{
    public static class BanPlayerCommand
    {
        public class Request : IRequest<string>
        {
            public string PlayerName { get; set; }
            public string Duration { get; set; }
            public string Reason { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.PlayerName).NotEmpty().MaximumLength(50);
                RuleFor(x => x.Duration).NotEmpty();
                RuleFor(x => x.Reason).MaximumLength(100);
            }
        }

        public class Handler : IRequestHandler<Request, string>
        {
            private readonly IRconService RconService;
            private readonly Command<SquadCommand> Command;

            public Handler(IRconService rconService, Command<SquadCommand> command)
            {
                RconService = rconService;
                Command = command;
            }

            public async Task<string> Handle(Request request, CancellationToken cancellationToken)
            {
                return await RconService.ExecuteCommandAsync(Command, SquadCommand.BanPlayer, request.PlayerName, request.Duration, request.Reason);
            }
        }
    }
}
