using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: ADMIN COMMAND: Message broadcasted <(.+)> from (.+)")]
    public class AdminBroadcastModel : IEventData
    {
        public string Time { get; set; }
        public string ChainID { get; set; }
        public string Message { get; set; }
        public string From { get; set; }
    }
}
