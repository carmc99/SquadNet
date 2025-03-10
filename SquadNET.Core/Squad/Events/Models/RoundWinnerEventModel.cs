// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[\s*([0-9]+)]LogSquadTrace: \[DedicatedServer](?:ASQGameMode::)?DetermineMatchWinner\(\): (.+?)(?: won on | on )(.+)$")]
    public class RoundWinnerEventModel : ISquadEventData
    {
        public string ChainID { get; set; }
        public string Layer { get; set; }
        public string Time { get; set; }
        public string Winner { get; set; }
    }
}