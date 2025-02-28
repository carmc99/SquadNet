
namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^\[(ChatSquad|ChatAdmin|ChatTeam|ChatAll)\] \[Online IDs:EOS: ([a-fA-F0-9]+) steam: ([0-9]+)\] (.+) : (.+)$")]
    public class ChatMessageInfo
    {
        public ChatChannelInfo Channel { get; set; }
        public CreatorOnlineIds CreatorIds { get; set; }
        public string PlayerName { get; set; }
        public string Message { get; set; }
    }
}
