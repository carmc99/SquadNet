// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadGameEvents: Display: Team ([0-9]+), (.*?) \( ?(.*?) ?\) has (won|lost) the match with ([0-9]+) Tickets on layer (.*?) \(level (.*?)\)!")]
    public class RoundTicketsEventModel : ISquadEventData
    {
        public string Action { get; set; }
        public string ChainID { get; set; }
        public string Faction { get; set; }
        public string Layer { get; set; }
        public string Level { get; set; }
        public string Subfaction { get; set; }
        public int Team { get; set; }
        public int Tickets { get; set; }
        public string Time { get; set; }
    }
}