using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Domain.Entities
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: Player:(.+) ActualDamage=([0-9.]+) from (.+) \(Online IDs:([^|]+)\| Player Controller ID: ([^ ]+)\)caused by ([A-z_0-9-]+)_C")]
    public class PlayerDamagedInfo
    {
        public RawDataInfo Raw { get; set; }
        public string Time { get; set; }
        public string ChainID { get; set; }
        public string VictimName { get; set; }
        public double Damage { get; set; }
        public string AttackerName { get; set; }
        public string AttackerController { get; set; }
        public string Weapon { get; set; }
        /// <summary>
        /// IDs adicionales del atacante (por ejemplo, attackerEos, attackerSteam, etc.)
        /// </summary>
        public Dictionary<string, string> AttackerIDs { get; set; } = new();
    }
}
