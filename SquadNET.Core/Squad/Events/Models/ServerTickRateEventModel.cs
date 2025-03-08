// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: USQGameState: Server Tick Rate: ([0-9.]+)")]
    public class ServerTickRateEventModel : ISquadEventData
    {
        public string ChainID { get; set; }
        public float TickRate { get; set; }
        public string Time { get; set; }
    }
}