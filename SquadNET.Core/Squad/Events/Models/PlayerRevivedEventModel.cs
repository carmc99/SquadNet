using SquadNET.Core.Squad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: (.+) \(Online IDs:([^)]+)\) has revived (.+) \(Online IDs:([^)]+)\)\.")]
    public class PlayerRevivedEventModel : IEventData
    {
        public string Time { get; set; }
        public string ChainID { get; set; }
        public string ReviverName { get; set; }
        public string VictimName { get; set; }
        public Dictionary<string, string> ReviverIDs { get; set; } = new();
        public Dictionary<string, string> VictimIDs { get; set; } = new();
    }
}
