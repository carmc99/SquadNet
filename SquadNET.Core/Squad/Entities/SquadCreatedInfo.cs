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
        public string PlayerName { get; }
        public CreatorOnlineIds CreatorIds { get; }
        public int SquadId { get; }
        public string SquadName { get; }
        public string TeamName { get; }

        public SquadCreatedInfo(string playerName, CreatorOnlineIds creatorIds, int squadId, string squadName, string teamName)
        {
            PlayerName = playerName;
            CreatorIds = creatorIds;
            SquadId = squadId;
            SquadName = squadName;
            TeamName = teamName;
        }

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
