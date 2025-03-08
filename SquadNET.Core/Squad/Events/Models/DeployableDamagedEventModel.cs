// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer](?:ASQDeployable::)?TakeDamage\(\): ([A-z0-9_]+)_C_[0-9]+: ([0-9.]+) damage attempt by causer ([A-z0-9_]+)_C_[0-9]+ instigator (.+) with damage type ([A-z0-9_]+)_C health remaining ([0-9.]+)")]
    public class DeployableDamagedEventModel : ISquadEventData
    {
        public string ChainID { get; set; }
        public float Damage { get; set; }
        public string DamageType { get; set; }
        public string Deployable { get; set; }
        public float HealthRemaining { get; set; }
        public string PlayerSuffix { get; set; }
        public string Time { get; set; }
        public string Weapon { get; set; }
    }
}