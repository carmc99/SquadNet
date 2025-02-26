using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquadTrace: \[DedicatedServer](?:ASQGameMode::)?DetermineMatchWinner\(\): (.+) won on (.+)")]
    public class RoundWinnerEventModel : IEventData
    {
        public string Time { get; set; }
        public string ChainID { get; set; }
        public string Winner { get; set; }
        public string Layer { get; set; }
    }
}
