// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^ID: ([0-9]+) \| SteamID: ([0-9]+) \| Since Disconnect: ([0-9]+)m\.([0-9]+)s \| Name: (.*)$")]
    public class PlayerDisconnectedEventModel
    {
        public TimeSpan DisconnectedSince { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public ulong SteamId { get; set; }

        public bool Equals(
            PlayerDisconnectedEventModel other
        )
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && SteamId == other.SteamId && DisconnectedSince.Equals(other.DisconnectedSince) && Name == other.Name;
        }

        public override bool Equals(
            object obj
        )
        {
            return ReferenceEquals(this, obj) || obj is PlayerDisconnectedEventModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, SteamId, DisconnectedSince, Name);
        }
    }
}