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
    /// <summary>
    /// Command to kick a player from the server with a specified reason.
    /// </summary>
    public static class KickPlayerCommand
    {
        public class Request : IRequest<string>
        {
            public string PlayerName { get; set; }
            public string Reason { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.PlayerName).NotEmpty().MaximumLength(50);
                RuleFor(x => x.Reason).NotEmpty().MaximumLength(100);
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
                return await RconService.ExecuteCommandAsync(Command, SquadCommand.KickPlayer, request.PlayerName, request.Reason);
            }
        }
    }

}
