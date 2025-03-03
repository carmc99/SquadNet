// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"{.*}")]
    public class ServerInformationInfo
    {
        /// <summary>
        /// The number of players reported by the A2S (Application to Server) query.
        /// </summary>
        public int A2sPlayerCount { get; set; }

        /// <summary>
        /// The name of the currently active map (layer) on the server.
        /// </summary>
        public string CurrentLayer { get; set; }

        /// <summary>
        /// The version of the Squad game currently running on the server.
        /// </summary>
        public string GameVersion { get; set; }

        /// <summary>
        /// The estimated start time of the match, calculated based on server uptime.
        /// </summary>
        public DateTime MatchStartTime { get; set; }

        /// <summary>
        /// The timeout duration of the current match, in seconds.
        /// </summary>
        public double MatchTimeout { get; set; }

        /// <summary>
        /// The maximum number of players allowed on the server.
        /// </summary>
        public int MaxPlayers { get; set; }

        /// <summary>
        /// The name of the next map (layer) that will be loaded after the current match ends.
        /// </summary>
        public string NextLayer { get; set; }

        /// <summary>
        /// The current number of players connected to the server.
        /// </summary>
        public int PlayerCount { get; set; }

        /// <summary>
        /// The number of players currently in the public queue waiting to join the server.
        /// </summary>
        public int PublicQueue { get; set; }

        /// <summary>
        /// The maximum limit of players allowed in the public queue.
        /// </summary>
        public int PublicQueueLimit { get; set; }

        /// <summary>
        /// The number of players currently in the reserved queue waiting to join the server.
        /// </summary>
        public int ReserveQueue { get; set; }

        /// <summary>
        /// The number of reserved slots for special players or administrators.
        /// </summary>
        public int ReserveSlots { get; set; }

        /// <summary>
        /// The name of the Squad server.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The name of the first team playing in the match.
        /// </summary>
        public string TeamOne { get; set; }

        /// <summary>
        /// The name of the second team playing in the match.
        /// </summary>
        public string TeamTwo { get; set; }
    }
}