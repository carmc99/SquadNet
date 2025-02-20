using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^(.*) \(Steam ID: ([0-9]+)\) has created Squad ([0-9]+) \(Squad Name: (.*)\) on (.*)$")]
    public class SquadCreatedInfo
    {
        public string PlayerNameWithoutPrefix { get; }
        public ulong PlayerSteamId { get; }
        public int SquadId { get; }
        public string SquadName { get; }
        public string TeamName { get; }
        public override bool Equals(object obj)
        {
            return obj is SquadCreatedInfo other &&
                   PlayerNameWithoutPrefix == other.PlayerNameWithoutPrefix &&
                   PlayerSteamId == other.PlayerSteamId &&
                   SquadId == other.SquadId &&
                   SquadName == other.SquadName &&
                   TeamName == other.TeamName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PlayerNameWithoutPrefix, PlayerSteamId, SquadId, SquadName, TeamName);
        }
    }
}
