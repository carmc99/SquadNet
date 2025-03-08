// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events.Models;

namespace SquadNET.Core.Squad.Models
{
    public sealed class ListPlayerModel
    {
        public ListPlayerModel(
            List<PlayerConnectedEventModel> activePlayers,
            List<PlayerDisconnectedEventModel> disconnectedPlayers
        )
        {
            ActivePlayers = activePlayers;
            DisconnectedPlayers = disconnectedPlayers;
        }

        public List<PlayerConnectedEventModel> ActivePlayers { get; }
        public List<PlayerDisconnectedEventModel> DisconnectedPlayers { get; }
    }
}