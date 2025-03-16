// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

namespace SquadNET.Core.Squad.Models
{
    [RegexPattern(@"^ID: ([0-9]+) \| Online IDs: EOS: ([0-9a-f]+) steam: ([0-9]+) \| Since Disconnect: ([0-9]+)m\.([0-9]+)s \| Name: (.*)$")]
    public class PlayerDisconnectedModel
    {
        public CreatorOnlineModel CreatorIds { get; set; }
        public TimeSpan DisconnectedSince { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Equals(PlayerDisconnectedModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && CreatorIds.Equals(other.CreatorIds) && DisconnectedSince.Equals(other.DisconnectedSince) && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is PlayerDisconnectedModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, CreatorIds, DisconnectedSince, Name);
        }
    }
}