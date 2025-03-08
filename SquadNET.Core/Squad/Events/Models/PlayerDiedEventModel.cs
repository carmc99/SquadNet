// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Models;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer](?:ASQSoldier::)?Die\(\): Player:(.+) KillingDamage=(?:-)*([0-9.]+) from ([A-z_0-9]+) \(Online IDs:([^)|]+)\| Contoller ID: ([\w\d]+)\) caused by ([A-z_0-9-]+)_C")]
    public class PlayerDiedEventModel : ISquadEventData
    {
        public CreatorOnlineModel AttackerIds { get; set; }
        public string AttackerPlayerController { get; set; }
        public int ChainID { get; set; }
        public float Damage { get; set; }
        public string Time { get; set; }
        public string VictimName { get; set; }
        public string Weapon { get; set; }
        public string WoundTime { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="PlayerDiedEventModel"/> from parsed log data.
        /// </summary>
        public static PlayerDiedEventModel FromParsedData(string time, string woundTime, int chainID, string victimName,
                                                          float damage, string attackerPlayerController, string onlineIds, string weapon)
        {
            return new PlayerDiedEventModel
            {
                Time = time,
                WoundTime = woundTime,
                ChainID = chainID,
                VictimName = victimName,
                Damage = damage,
                AttackerPlayerController = attackerPlayerController,
                Weapon = weapon,
                AttackerIds = CreatorOnlineModel.FromString(onlineIds)
            };
        }
    }
}