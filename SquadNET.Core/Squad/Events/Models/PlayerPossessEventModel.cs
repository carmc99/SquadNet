using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer](?:ASQPlayerController::)?OnPossess\(\): PC=(.+) \(Online IDs:([^)]+)\) Pawn=([A-z0-9_]+)_C")]
    public class PlayerPossessEventModel : ISquadEventData
    {
        public string Time { get; set; }
        public string ChainID { get; set; }
        public string PlayerSuffix { get; set; }
        public string PossessClassname { get; set; }
        public Dictionary<string, string> PlayerIDs { get; set; } = new();
    }
}
