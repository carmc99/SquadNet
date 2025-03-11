// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Admin.Queries
{
    /// <summary>
    /// Query to list all available RCON commands.
    /// </summary>
    public static class ListCommandsQuery
    {
        public class Handler : IRequestHandler<Request, List<CommandModel>>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<List<CommandModel>> Parser;
            private readonly IRconService RconService;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<List<CommandModel>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<List<CommandModel>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListCommands, cancellationToken);
                List<CommandModel> commands = Parser.Parse(result);
                return commands;
            }
        }

        public class Request : IRequest<List<CommandModel>>
        { }
    }
}