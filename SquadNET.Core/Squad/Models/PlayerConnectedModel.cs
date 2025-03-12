// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

namespace SquadNET.Core.Squad.Models
{
    [RegexPattern(@"^ID: (\d+) \| Online IDs: EOS: ([0-9a-f]+) steam: (\d+) \| Name: (.+?) \| Team ID: (\d+) \| Squad ID: (N/A|\d+) \| Is Leader: (False|True) \| Role: ([A-Za-z0-9_-]+)$")]
    public class PlayerConnectedModel
    {
        public string CreatorId { get; set; }
        public CreatorOnlineModel CreatorIds { get; set; }
        public int Id { get; set; }
        public bool IsLeader { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int? SquadId { get; set; }
        public TeamType Team { get; set; }

        public bool Equals(PlayerConnectedModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && CreatorIds.Equals(other.CreatorIds) && Name == other.Name &&
                   Team == other.Team && IsLeader == other.IsLeader && Role == other.Role && SquadId == other.SquadId;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is PlayerConnectedModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, CreatorIds, Name, (int)Team, IsLeader, Role, SquadId);
        }
    }
}