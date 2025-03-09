// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Models;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer](?:ASQPlayerController::)?OnPossess\(\): PC=(.+) \(Online IDs: ([^)]+)\) Pawn=([A-z0-9_]+)_C")]
    public class PlayerPossessEventModel : ISquadEventData
    {
        public string ChainID { get; set; }
        public CreatorOnlineModel CreatorIds { get; set; }
        public string PlayerSuffix { get; set; }
        public string PossessClassname { get; set; }
        public string Time { get; set; }

        public static PlayerPossessEventModel FromParsedData(
            string time, string chainID, string playerSuffix, string onlineIDs, string possessClassname)
        {
            return new PlayerPossessEventModel
            {
                Time = time,
                ChainID = chainID,
                PlayerSuffix = playerSuffix,
                CreatorIds = CreatorOnlineModel.FromString(onlineIDs),
                PossessClassname = possessClassname
            };
        }
    }
}