// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Map.Repositories.EF
{
    public interface IMapRepository
    {
        Task Store(List<LayerModel> model);
    }
}