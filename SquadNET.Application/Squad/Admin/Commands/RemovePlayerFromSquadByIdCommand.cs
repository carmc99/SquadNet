﻿using FluentValidation;
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
    /// Command to remove a player from their squad by ID.
    /// </summary>
    public static class RemovePlayerFromSquadByIdCommand
    {
        public class Request : IRequest<string>
        {
            public int PlayerId { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(x => x.PlayerId).GreaterThan(0);
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
                return await RconService.ExecuteCommandAsync(Command, SquadCommand.RemovePlayerFromSquadById, request.PlayerId);
            }
        }
    }
}
