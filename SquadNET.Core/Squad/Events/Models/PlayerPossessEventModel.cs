// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Models;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer\]ASQPlayerController::OnPossess\(\): PC=([^\s]+) \(Online IDs: EOS: ([^\s]+) steam: (\d+)\) (?:Entered Vehicle )?Pawn=([^\s]+)(?: \(Asset Name = ([^\s]+)\))?")]
    public class PlayerPossessEventModel : ISquadEventData
    {
        public int ChainID { get; set; }
        public CreatorOnlineModel CreatorIds { get; set; }
        public string PlayerName { get; set; }
        public string PossessClassname { get; set; }
        public string Time { get; set; }

        public static PlayerPossessEventModel FromParsedData(
            string time, int chainID, string playerName, string onlineIDs, string possessClassname)
        {
            return new PlayerPossessEventModel
            {
                Time = time,
                ChainID = chainID,
                PlayerName = playerName,
                CreatorIds = CreatorOnlineModel.FromString(onlineIDs),
                PossessClassname = possessClassname
            };
        }
    }
}