// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Models
{
    public sealed class ListPlayerModel
    {
        public ListPlayerModel(
            List<PlayerConnectedModel> activePlayers,
            List<PlayerDisconnectedModel> disconnectedPlayers
        )
        {
            ActivePlayers = activePlayers;
            DisconnectedPlayers = disconnectedPlayers;
        }

        public List<PlayerConnectedModel> ActivePlayers { get; }
        public List<PlayerDisconnectedModel> DisconnectedPlayers { get; }
    }
}