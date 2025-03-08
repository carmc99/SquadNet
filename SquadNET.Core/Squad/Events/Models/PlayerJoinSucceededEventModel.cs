// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogNet: Join succeeded: (.+)")]
    public class PlayerJoinSucceededEventModel : ISquadEventData
    {
        /// <summary>
        /// The unique event chain ID.
        /// </summary>
        public string ChainID { get; set; }

        /// <summary>
        /// The name or identifier of the player who successfully joined.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// The timestamp of the event.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Parses log data to create an instance of <see cref="PlayerJoinSucceededEventModel"/>.
        /// </summary>
        public static PlayerJoinSucceededEventModel FromParsedData(string time, string chainID, string playerName)
        {
            return new PlayerJoinSucceededEventModel
            {
                Time = time,
                ChainID = chainID,
                PlayerName = playerName
            };
        }

        /// <summary>
        /// Returns a formatted string representation of the event.
        /// </summary>
        public override string ToString()
        {
            return $"[{Time}] Player '{PlayerName}' successfully joined (Chain ID: {ChainID}).";
        }
    }
}