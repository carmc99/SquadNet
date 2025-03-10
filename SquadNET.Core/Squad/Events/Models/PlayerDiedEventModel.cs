// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Models;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"\[.*?\]LogSquadTrace: \[DedicatedServer\]ASQSoldier::Die\(\): Player: ([^ ]+) KillingDamage=([-\d.]+) from ([^ ]+) \(Online IDs: EOS: ([0-9a-f]+) steam: (\d+) \| Contoller ID: ([^ ]+)\) caused by ([^ ]+)")]
    public class PlayerDiedEventModel : ISquadEventData
    {
        public CreatorOnlineModel CreatorIds { get; set; }
        public string KillerControllerId { get; set; }
        public string KillerName { get; set; }
        public float KillingDamage { get; set; }
        public string VictimName { get; set; }
        public string Weapon { get; set; }

        public static PlayerDiedEventModel FromParsedData(
            string victimName, float killingDamage, string killerName,
            string killerEosId, ulong killerSteamId, string killerControllerId, string weapon)
        {
            return new PlayerDiedEventModel
            {
                VictimName = victimName,
                KillingDamage = killingDamage,
                KillerName = killerName,
                CreatorIds = CreatorOnlineModel.FromString($"EOS: {killerEosId} steam: {killerSteamId}"),
                KillerControllerId = killerControllerId,
                Weapon = weapon
            };
        }
    }
}