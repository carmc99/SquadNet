// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer](?:ASQSoldier::)?Wound\(\): Player:(.+) KillingDamage=(?:-)*([0-9.]+) from ([A-z_0-9]+) \(Online IDs:([^)|]+)\| Controller ID: ([\w\d]+)\) caused by ([A-z_0-9-]+)_C")]
    public class PlayerWoundedEventModel : ISquadEventData
    {
        public CreatorOnlineIds AttackerIds { get; set; }
        public string AttackerPlayerController { get; set; }
        public int ChainID { get; set; }
        public float Damage { get; set; }
        public string Time { get; set; }
        public string VictimName { get; set; }
        public string Weapon { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="PlayerWoundedEventModel"/> from parsed log data.
        /// </summary>
        public static PlayerWoundedEventModel FromParsedData(
            string time, int chainID, string victimName, float damage,
            string attackerPlayerController, string onlineIds, string weapon)
        {
            return new PlayerWoundedEventModel
            {
                Time = time,
                ChainID = chainID,
                VictimName = victimName,
                Damage = damage,
                AttackerPlayerController = attackerPlayerController,
                Weapon = weapon,
                AttackerIds = CreatorOnlineIds.FromString(onlineIds)
            };
        }
    }
}