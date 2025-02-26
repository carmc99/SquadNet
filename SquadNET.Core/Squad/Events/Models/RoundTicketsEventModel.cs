using SquadNET.Core.Squad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadGameEvents: Display: Team ([0-9]), (.*) \( ?(.*?) ?\) has (won|lost) the match with ([0-9]+) Tickets on layer (.*) \(level (.*)\)!")]
    public class RoundTicketsEventModel : IEventData
    {
        public string Time { get; set; }
        public string ChainID { get; set; }
        public int Team { get; set; }
        public string Subfaction { get; set; }
        public string Faction { get; set; }
        public string Action { get; set; }
        public int Tickets { get; set; }
        public string Layer { get; set; }
        public string Level { get; set; }
    }
}
