// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer](?:ASQDeployable::)?TakeDamage\(\): ([A-z0-9_]+)_C_[0-9]+: ([0-9.]+) damage attempt by causer ([A-z0-9_]+)_C_[0-9]+ instigator (.+) with damage type ([A-z0-9_]+)_C health remaining ([0-9.]+)")]
    public class DeployableDamagedEventModel : ISquadEventData
    {
        /// <summary>
        /// The online identifiers (EOS, Steam) of the player who caused the damage.
        /// </summary>
        public CreatorOnlineIds AttackerIds { get; set; }

        /// <summary>
        /// The unique event chain ID.
        /// </summary>
        public string ChainID { get; set; }

        /// <summary>
        /// The amount of damage dealt to the deployable.
        /// </summary>
        public float Damage { get; set; }

        /// <summary>
        /// The damage type that was used.
        /// </summary>
        public string DamageType { get; set; }

        /// <summary>
        /// The name of the deployable structure that was damaged.
        /// </summary>
        public string Deployable { get; set; }

        /// <summary>
        /// The remaining health of the deployable after taking damage.
        /// </summary>
        public float HealthRemaining { get; set; }

        /// <summary>
        /// The timestamp of the event.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// The name of the weapon used to deal damage.
        /// </summary>
        public string Weapon { get; set; }

        /// <summary>
        /// Parses log data to create an instance of <see cref="DeployableDamagedEventModel"/>.
        /// </summary>
        public static DeployableDamagedEventModel FromParsedData(string time, string chainID, string deployable,
                                                                 float damage, string weapon, string attackerIds,
                                                                 string damageType, float healthRemaining)
        {
            return new DeployableDamagedEventModel
            {
                Time = time,
                ChainID = chainID,
                Deployable = deployable,
                Damage = damage,
                Weapon = weapon,
                AttackerIds = CreatorOnlineIds.FromString(attackerIds),
                DamageType = damageType,
                HealthRemaining = healthRemaining
            };
        }

        /// <summary>
        /// Returns a formatted string representation of the event.
        /// </summary>
        public override string ToString()
        {
            return $"[{Time}] Deployable '{Deployable}' took {Damage} damage from {AttackerIds} using {Weapon} (Type: {DamageType}). " +
                   $"Remaining health: {HealthRemaining}";
        }
    }
}