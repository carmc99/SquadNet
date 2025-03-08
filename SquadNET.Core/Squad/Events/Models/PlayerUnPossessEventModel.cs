// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer](?:ASQPlayerController::)?OnUnPossess\(\): PC=(.+) \(Online IDs:([^)]+)\)")]
    public class PlayerUnPossessEventModel : ISquadEventData
    {
        public string ChainID { get; set; }
        public CreatorOnlineIds PlayerIds { get; set; }

        public string PlayerSuffix { get; set; }

        public bool SwitchPossess { get; set; }
        public string Time { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="PlayerUnPossessEventModel"/> from parsed log data.
        /// </summary>
        public static PlayerUnPossessEventModel FromParsedData(string time, string chainID, string playerSuffix, string onlineIds, bool switchPossess)
        {
            return new PlayerUnPossessEventModel
            {
                Time = time,
                ChainID = chainID,
                PlayerSuffix = playerSuffix,
                PlayerIds = CreatorOnlineIds.FromString(onlineIds),
                SwitchPossess = switchPossess
            };
        }
    }
}