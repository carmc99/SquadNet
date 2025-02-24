using SquadNET.Core.Squad.Entities;
using System;

namespace SquadNET.Core.Squad.Models
{
    /// <summary>
    /// Represents a chat message model, used for storage and serialization.
    /// </summary>
    public class ChatMessageModel : IEventData
    {
        /// <summary>
        /// The chat channel where the message was sent.
        /// </summary>
        public ChatChannelInfo Channel { get; set; }

        /// <summary>
        /// The EOS (Epic Online Services) ID of the player.
        /// </summary>
        public string EosId { get; set; }

        /// <summary>
        /// The Steam ID of the player.
        /// </summary>
        public ulong SteamId { get; set; }

        /// <summary>
        /// The name of the player who sent the message.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// The message content.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The timestamp indicating when the message was received.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Returns a formatted string representation of the chat message.
        /// </summary>
        public override string ToString()
        {
            return $"[{Timestamp}] {Channel} | Player: {PlayerName} (EOS: {EosId}, Steam: {SteamId}) | Message: {Message}";
        }

        /// <summary>
        /// Converts a `ChatMessageInfo` entity into a `ChatMessageInfoModel` for storage.
        /// </summary>
        /// <param name="entity">The `ChatMessageInfo` entity to convert.</param>
        /// <returns>A new instance of `ChatMessageInfoModel`.</returns>
        public static ChatMessageModel FromEntity(ChatMessageInfo entity)
        {
            return new ChatMessageModel
            {
                Channel = entity.Channel,
                EosId = entity.CreatorIds.EosId,
                SteamId = entity.CreatorIds.SteamId,
                PlayerName = entity.PlayerName,
                Message = entity.Message,
                Timestamp = DateTime.UtcNow // Stores the message time in UTC
            };
        }

        /// <summary>
        /// Converts the `ChatMessageInfoModel` back into a `ChatMessageInfo` entity.
        /// </summary>
        /// <returns>A `ChatMessageInfo` entity.</returns>
        public ChatMessageInfo ToEntity()
        {
            return new ChatMessageInfo
            {
                Channel = this.Channel,
                CreatorIds = new CreatorOnlineIds(EosId, SteamId),
                PlayerName = this.PlayerName,
                Message = this.Message
            };
        }
    }
}
