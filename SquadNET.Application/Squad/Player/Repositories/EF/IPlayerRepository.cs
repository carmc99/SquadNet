// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Application.Squad.Player.Repositories.EF
{
    public interface IPlayerRepository
    {
        Task Store(ListPlayerModel model);
    }
}