using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^(.+) \(Online IDs: EOS: ([0-9a-f]+) steam: (\d+)\) has created Squad (\d+) \(Squad Name: (.+)\) on (.+)$")]
    public class SquadCreatedInfo
    {
        public string PlayerName { get; set; }
        public CreatorOnlineIds CreatorIds { get; set; }
        public int SquadId { get; set; }
        public string SquadName { get; set; }
        public string TeamName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SquadCreatedInfo other &&
                   PlayerName == other.PlayerName &&
                   CreatorIds.Equals(other.CreatorIds) &&
                   SquadId == other.SquadId &&
                   SquadName == other.SquadName &&
                   TeamName == other.TeamName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PlayerName, CreatorIds, SquadId, SquadName, TeamName);
        }
    }

}
