using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: USQGameState: Server Tick Rate: ([0-9.]+)")]
    public class ServerTickRateEventModel : IEventData
    {
        public string Time { get; set; }
        public string ChainID { get; set; }
        public float TickRate { get; set; }
    }
}
