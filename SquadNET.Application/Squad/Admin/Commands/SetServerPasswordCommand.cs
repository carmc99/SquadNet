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
    /// Command to set or remove the server password.
    /// </summary>
    public static class SetServerPasswordCommand
    {
        public class Request : IRequest<string>
        {
            public string Password { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.Password).MaximumLength(50);
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
                return await RconService.ExecuteCommandAsync(Command, SquadCommand.SetServerPassword, request.Password);
            }
        }
    }
}
