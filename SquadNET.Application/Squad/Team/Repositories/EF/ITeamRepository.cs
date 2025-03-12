// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Team.Repositories.EF
{
    public interface ITeamRepository
    {
        Task Store(List<TeamModel> model);
    }
}