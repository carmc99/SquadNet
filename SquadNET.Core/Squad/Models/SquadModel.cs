// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Models
{
    [RegexPattern(@"^ID: ([0-9]+) \| Name: (.+?) \| Size: (\d+) \| Locked: (True|False) \| Creator Name: (.+?) \| Creator Online IDs: EOS: ([0-9a-f]+) steam: (\d+)$")]
    public class SquadModel
    {
        public CreatorOnlineModel CreatorIds { get; set; }
        public string CreatorName { get; set; }
        public int Id { get; set; }
        public bool IsLocked { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public TeamType TeamId { get; set; }
        public string TeamName { get; set; }

        public bool Equals(SquadModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && TeamId == other.TeamId && TeamName == other.TeamName &&
                   Name == other.Name && Size == other.Size && CreatorName == other.CreatorName &&
                   CreatorIds.Equals(other.CreatorIds) && IsLocked == other.IsLocked;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is SquadModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, (int)TeamId, TeamName, Name, Size, CreatorName, CreatorIds, IsLocked);
        }
    }
}