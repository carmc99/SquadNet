using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: USQGameState: Server Tick Rate: ([0-9.]+)")]
    public class ServerTickRateEventModel : ISquadEventData
    {
        public string Time { get; set; }
        public string ChainID { get; set; }
        public float TickRate { get; set; }
    }
}
