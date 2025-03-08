// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: Player:(.+) ActualDamage=([0-9.]+) from (.+) \(Online IDs:([^|]+)\| Player Controller ID: ([^ ]+)\)caused by ([A-z_0-9-]+)_C")]
    public class PlayerDamagedEventModel : ISquadEventData
    {
        public string AttackerController { get; set; }
        public CreatorOnlineIds AttackerIds { get; set; }
        public string AttackerName { get; set; }
        public string ChainID { get; set; }
        public float Damage { get; set; }
        public string Time { get; set; }
        public string VictimName { get; set; }
        public string Weapon { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="PlayerDamagedEventModel"/> from parsed log data.
        /// </summary>
        public static PlayerDamagedEventModel FromParsedData(
            string time, string chainID, string victimName, float damage,
            string attackerName, string onlineIds, string attackerController, string weapon)
        {
            return new PlayerDamagedEventModel
            {
                Time = time,
                ChainID = chainID,
                VictimName = victimName,
                Damage = damage,
                AttackerName = attackerName,
                AttackerController = attackerController,
                Weapon = weapon,
                AttackerIds = CreatorOnlineIds.FromString(onlineIds)
            };
        }
    }
}