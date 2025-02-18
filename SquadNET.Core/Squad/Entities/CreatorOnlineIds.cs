using System;
using System.Collections.Generic;

namespace SquadNET.Core.Squad.Entities
{
    public class CreatorOnlineIds
    {
        public string EosId { get; set; }
        public ulong SteamId { get; set; }

        public CreatorOnlineIds(string eosId, ulong steamId)
        {
            EosId = eosId;
            SteamId = steamId;
        }

        public bool Equals(CreatorOnlineIds? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EosId == other.EosId && SteamId == other.SteamId;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is CreatorOnlineIds other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EosId, SteamId);
        }
    }
}
