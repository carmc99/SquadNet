// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer](?:ASQPlayerController::)?OnUnPossess\(\): PC=(.+) \(Online IDs:([^)]+)\)")]
    public class PlayerUnPossessEventModel : ISquadEventData
    {
        public string ChainID { get; set; }
        public Dictionary<string, string> PlayerIDs { get; set; } = new();
        public string PlayerSuffix { get; set; }
        public bool SwitchPossess { get; set; }
        public string Time { get; set; }
    }
}