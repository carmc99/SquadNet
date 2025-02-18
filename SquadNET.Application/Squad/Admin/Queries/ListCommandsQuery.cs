using MediatR;
using Squadmania.Squad.Rcon.Parsers;
using SquadNET.Core;
using SquadNET.Rcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Admin.Queries
{
    /// <summary>
    /// Query to list all available RCON commands.
    /// </summary>
    public static class ListCommandsQuery
    {
        public class Request : IRequest<List<string>> { }

        public class Handler : IRequestHandler<Request, List<string>>
        {
            private readonly IRconService RconService;
            private readonly Command<SquadCommand> Command;

            public Handler(IRconService rconService, Command<SquadCommand> command)
            {
                RconService = rconService;
                Command = command;
            }

            public async Task<List<string>> Handle(Request request, CancellationToken cancellationToken)
            {
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ListCommands, cancellationToken);

                return null;
                //TODO: return ListCommandsParser.Parse(result);
            }
        }
    }
}
