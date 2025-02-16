using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Domain.Entities
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer](?:ASQDeployable::)?TakeDamage\(\): ([A-z0-9_]+)_C_[0-9]+: ([0-9.]+) damage attempt by causer ([A-z0-9_]+)_C_[0-9]+ instigator (.+) with damage type ([A-z0-9_]+)_C health remaining ([0-9.]+)")]
    public class DeployableDamagedInfo
    {
        public RawDataInfo Raw { get; set; }
        public string Time { get; set; }
        public string ChainID { get; set; }
        public string Deployable { get; set; }
        public double Damage { get; set; }
        public string Weapon { get; set; }
        public string PlayerSuffix { get; set; }
        public string DamageType { get; set; }
        public double HealthRemaining { get; set; }
    }
}
