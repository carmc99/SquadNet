using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^ID: ([0-9]+) \| SteamID: ([0-9]+) \| Since Disconnect: ([0-9]+)m\.([0-9]+)s \| Name: (.*)$")]
    public class PlayerDisconnectedInfo
    {
        public PlayerDisconnectedInfo(
            int id,
            ulong steamId64,
            TimeSpan disconnectedSince,
            string name
        )
        {
            Id = id;
            SteamId64 = steamId64;
            DisconnectedSince = disconnectedSince;
            Name = name;
        }

        public int Id { get; }
        public ulong SteamId64 { get; }
        public TimeSpan DisconnectedSince { get; }
        public string Name { get; }

        public bool Equals(
            PlayerDisconnectedInfo? other
        )
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && SteamId64 == other.SteamId64 && DisconnectedSince.Equals(other.DisconnectedSince) && Name == other.Name;
        }

        public override bool Equals(
            object? obj
        )
        {
            return ReferenceEquals(this, obj) || obj is PlayerDisconnectedInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, SteamId64, DisconnectedSince, Name);
        }
    }
}
