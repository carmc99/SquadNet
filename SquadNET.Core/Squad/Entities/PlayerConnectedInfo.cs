﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^ID: (\d+) \| Online IDs: EOS: ([0-9a-f]+) steam: (\d+) \| Name: (.+?) \| Team ID: (\d+) \| Squad ID: (N/A|\d+) \| Is Leader: (False|True) \| Role: ([A-Za-z0-9_-]+)$")]
    public class PlayerConnectedInfo
    {
        public int Id { get; set; }
        public string EosId { get; set; }
        public ulong SteamId { get; set; }
        public string Name { get; set; }
        public TeamId Team { get; set; }
        public bool IsLeader { get; set; }
        public string Role { get; set; }
        public int? SquadId { get; set; }

        public bool Equals(
            PlayerConnectedInfo? other
        )
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && SteamId == other.SteamId && Name == other.Name && Team == other.Team && IsLeader == other.IsLeader && Role == other.Role && SquadId == other.SquadId;
        }

        public override bool Equals(
            object? obj
        )
        {
            return ReferenceEquals(this, obj) || obj is PlayerConnectedInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, SteamId, Name, (int)Team, IsLeader, Role, SquadId);
        }
    }
}
