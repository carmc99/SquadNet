using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^\[(ChatSquad|ChatAdmin|ChatTeam|ChatAll)\] \[SteamID:([0-9]+)\] (.*) : (.*)$")]
    public class ChatMessageInfo
    {
        public ChatChannelInfo Channel { get; set; }
        public ulong PlayerSteamId64 { get; set; }
        public string PlayerName { get; set; }
        public string Message { get; set; }
    }
}
