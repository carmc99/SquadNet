using SquadNET.Core.Squad.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Models
{
    public sealed class ListPlayerModel
    {
        public ListPlayerModel(
            List<PlayerConnectedInfo> activePlayers,
            List<PlayerDisconnectedInfo> disconnectedPlayers
        )
        {
            ActivePlayers = activePlayers;
            DisconnectedPlayers = disconnectedPlayers;
        }

        public List<PlayerConnectedInfo> ActivePlayers { get; }
        public List<PlayerDisconnectedInfo> DisconnectedPlayers { get; }
    }
}
