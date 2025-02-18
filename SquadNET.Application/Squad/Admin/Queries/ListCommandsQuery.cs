using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Admin.Queries
{
    /// <summary>
    /// Query to list all available RCON commands.
    /// </summary>
    public static class ListCommandsQuery
    {
        public class Request : IRequest<List<CommandInfo>> { }

        public class Handler : IRequestHandler<Request, List<CommandInfo>>
        {
            private readonly IRconService RconService;
            private readonly Command<SquadCommand> Command;
            private readonly ICommandParser<List<CommandInfo>> Parser;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                ICommandParser<List<CommandInfo>> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<List<CommandInfo>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListCommands, cancellationToken);
                List<CommandInfo> commands = Parser.Parse(result);
                return commands;
            }
        }
    }
}
