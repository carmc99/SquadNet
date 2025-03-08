// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Models;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Server.Queries
{
    public static class ServerInformationQuery
    {
        public class Handler : IRequestHandler<Request, ServerInformationModel>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<ServerInformationModel> Parser;
            private readonly IRconService RconService;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<ServerInformationModel> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<ServerInformationModel> Handle(Request request, CancellationToken cancellationToken)
            {
                ServerInformationModel serverInfo = new();
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ShowServerInfo, cancellationToken);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    serverInfo = Parser.Parse(result);
                }

                return serverInfo;
            }
        }

        public class Request : IRequest<ServerInformationModel>
        {
        }
    }
}