// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"\[.*?\]LogSquadTrace: \[DedicatedServer\]ASQSoldier::Wound\(\): Player: ([^ ]+) KillingDamage=([-\d.]+) from ([^ ]+) \(Online IDs: EOS: ([0-9a-f]+) steam: (\d+) \| Controller ID: ([^ ]+)\) caused by ([^ ]+)")]
    public class PlayerWoundedEventModel : ISquadEventData
    {
        public string KillerControllerId { get; set; }
        public string KillerEosId { get; set; }
        public string KillerName { get; set; }
        public ulong KillerSteamId { get; set; }
        public float KillingDamage { get; set; }
        public string VictimName { get; set; }
        public string Weapon { get; set; }

        public static PlayerWoundedEventModel FromParsedData(
            string victimName, float killingDamage, string killerName,
            string killerEosId, ulong killerSteamId, string killerControllerId, string weapon)
        {
            return new PlayerWoundedEventModel
            {
                VictimName = victimName,
                KillingDamage = killingDamage,
                KillerName = killerName,
                KillerEosId = killerEosId,
                KillerSteamId = killerSteamId,
                KillerControllerId = killerControllerId,
                Weapon = weapon
            };
        }
    }
}