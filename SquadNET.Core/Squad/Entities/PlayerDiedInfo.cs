using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer](?:ASQSoldier::)?Die\(\): Player:(.+) KillingDamage=(?:-)*([0-9.]+) from ([A-z_0-9]+) \(Online IDs:([^)|]+)\| Contoller ID: ([\w\d]+)\) caused by ([A-z_0-9-]+)_C")]
    public class PlayerDiedInfo
    {
        public RawDataInfo Raw { get; set; }
        public string Time { get; set; }
        /// <summary>
        /// Se utiliza el mismo valor que Time para indicar el instante en que se produjo la herida.
        /// </summary>
        public string WoundTime { get; set; }
        public string ChainID { get; set; }
        public string VictimName { get; set; }
        public double Damage { get; set; }
        public string AttackerPlayerController { get; set; }
        public string Weapon { get; set; }
        /// <summary>
        /// IDs adicionales del atacante (por ejemplo, attackerEos, attackerSteam, etc.)
        /// </summary>
        public Dictionary<string, string> AttackerIDs { get; set; } = new();
    }
}
