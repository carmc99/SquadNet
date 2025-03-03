// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using MediatR;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Rcon;

namespace SquadNET.Application.Squad.Server.Queries
{
    public static class ServerInformationQuery
    {
        public class Handler : IRequestHandler<Request, ServerInformationInfo>
        {
            private readonly Command<SquadCommand> Command;
            private readonly IParser<ServerInformationInfo> Parser;
            private readonly IRconService RconService;

            public Handler(IRconService rconService,
                Command<SquadCommand> command,
                IParser<ServerInformationInfo> parser)
            {
                RconService = rconService;
                Command = command;
                Parser = parser;
            }

            public async Task<ServerInformationInfo> Handle(Request request, CancellationToken cancellationToken)
            {
                ServerInformationInfo serverInfo = new();
                string result = await RconService.ExecuteCommandAsync(Command, SquadCommand.ShowServerInfo, cancellationToken);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    serverInfo = Parser.Parse(result);
                }

                return serverInfo;
            }
        }

        public class Request : IRequest<ServerInformationInfo>
        {
        }
    }
}