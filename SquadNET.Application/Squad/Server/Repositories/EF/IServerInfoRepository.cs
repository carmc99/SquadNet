// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Server.Repositories.EF
{
    public interface IServerInfoRepository
    {
        Task Store(ServerInformationModel model);
    }
}