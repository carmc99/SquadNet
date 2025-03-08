// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^(.+) \(Online IDs: EOS: ([0-9a-f]+) steam: (\d+)\) has created Squad (\d+) \(Squad Name: (.+)\) on (.+)$")]
    public class SquadCreatedEventModel : ISquadEventData
    {
        public CreatorOnlineIds CreatorIds { get; set; }
        public string PlayerName { get; set; }
        public int SquadId { get; set; }
        public string SquadName { get; set; }
        public string TeamName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SquadCreatedEventModel other &&
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