// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[(ChatSquad|ChatAdmin|ChatTeam|ChatAll)\] \[Online IDs:EOS: ([a-fA-F0-9]+) steam: ([0-9]+)\] (.+) : (.+)$")]
    public class ChatMessageEventModel : ISquadEventData
    {
        /// <summary>
        /// The chat channel where the message was sent.
        /// </summary>
        public ChatChannelInfo Channel { get; set; }

        /// <summary>
        /// The creator's online identifiers (EOS and Steam).
        /// </summary>
        public CreatorOnlineIds CreatorIds { get; set; }

        /// <summary>
        /// The content of the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The name of the player who sent the message.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// The timestamp indicating when the message was received.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Parses log data to extract a chat message event.
        /// </summary>
        public static ChatMessageEventModel FromLogData(string channel, string eosId, string steamId, string playerName, string message)
        {
            return new ChatMessageEventModel
            {
                Channel = Enum.TryParse(channel, out ChatChannelInfo parsedChannel) ? parsedChannel : ChatChannelInfo.ChatAll,
                CreatorIds = new CreatorOnlineIds(eosId, ulong.TryParse(steamId, out ulong parsedSteamId) ? parsedSteamId : 0),
                PlayerName = playerName,
                Message = message,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Returns a formatted string representation of the chat message.
        /// </summary>
        public override string ToString()
        {
            return $"[{Timestamp}] {Channel} | Player: {PlayerName} {CreatorIds} | Message: {Message}";
        }
    }
}