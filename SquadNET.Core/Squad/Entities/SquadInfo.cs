using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^ID: ([0-9]+) \| Name: (.+?) \| Size: (\d+) \| Locked: (True|False) \| Creator Name: (.+?) \| Creator Online IDs: EOS: ([0-9a-f]+) steam: (\d+)$")]
    public class SquadInfo
    {
        public int Id { get; set; }
        public TeamId TeamId { get; set; }
        public string TeamName { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string CreatorName { get; set; }
        public CreatorOnlineIds CreatorIds { get; set; }
        public ulong SteamId { get; set; }
        public bool IsLocked { get; set; }

        public bool Equals(SquadInfo? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && TeamId == other.TeamId && TeamName == other.TeamName &&
                   Name == other.Name && Size == other.Size && CreatorName == other.CreatorName &&
                   CreatorIds.Equals(other.CreatorIds) && IsLocked == other.IsLocked;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is SquadInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, (int)TeamId, TeamName, Name, Size, CreatorName, CreatorIds, IsLocked);
        }
    }
}
