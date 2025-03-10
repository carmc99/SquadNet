// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Models;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[(\d+)]LogSquad: Player:\s*(.+?)\s*ActualDamage=([0-9.]+) from (.+) \(Online IDs: ([^|)]+)\| Player Controller ID: ([^ ]+)\)caused by ([A-Za-z_0-9-]+_C_\d+)")]
    public class PlayerDamagedEventModel : ISquadEventData
    {
        public CreatorOnlineModel AttackerIds { get; set; }
        public string AttackerName { get; set; }
        public string AttackerPlayerController { get; set; }
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
            string attackerName, string onlineIds, string attackerPlayerController, string weapon)
        {
            return new PlayerDamagedEventModel
            {
                Time = time,
                ChainID = chainID,
                VictimName = victimName,
                Damage = damage,
                AttackerName = attackerName,
                AttackerPlayerController = attackerPlayerController,
                Weapon = weapon,
                AttackerIds = CreatorOnlineModel.FromString(onlineIds)
            };
        }
    }
}