using Squadmania.Squad.Rcon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^\[(ChatSquad|ChatAdmin|ChatTeam|ChatAll)\] \[Online IDs:EOS: ([a-fA-F0-9]+) steam: ([0-9]+)\] (.+) : (.+)$")]
    public class ChatMessageInfo
    {
        public ChatChannelInfo Channel { get; set; }
        public CreatorOnlineIds CreatorIds { get; set; }
        public string PlayerName { get; set; }
        public string Message { get; set; }

        public static ChatMessageInfo Convert(ChatMessage chatMessage)
        {
            return new ChatMessageInfo
            {
                Channel = (ChatChannelInfo)chatMessage.Channel,
                Message = chatMessage.Message,
                PlayerName = chatMessage.PlayerName,
                CreatorIds = new CreatorOnlineIds(
                    eosId: "UNKNOWN", // No se tiene EOS ID en ChatMessage
                    steamId: chatMessage.PlayerSteamId64 // Se usa Steam ID directamente
                )
            };
        }
    }
}
