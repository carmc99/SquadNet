using SquadNET.Core.Squad.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Models
{
    public class SquadCreatedEventModel : IEventData
    {
        /// <summary>
        /// The EOS (Epic Online Services) ID of the player who created the squad.
        /// </summary>
        public string EosId { get; set; }

        /// <summary>
        /// The Steam ID of the player who created the squad.
        /// </summary>
        public ulong SteamId { get; set; }

        /// <summary>
        /// The name of the player who created the squad.
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// The unique ID assigned to the squad.
        /// </summary>
        public int SquadId { get; set; }

        /// <summary>
        /// The name of the squad.
        /// </summary>
        public string SquadName { get; set; }

        /// <summary>
        /// The name of the team to which the squad belongs.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// The timestamp indicating when the squad was created.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Returns a formatted string representation of the squad information.
        /// </summary>
        public override string ToString()
        {
            return $"[{Timestamp}] Squad {SquadId} - '{SquadName}' created by {PlayerName} (EOS: {EosId}, Steam: {SteamId}) on {TeamName}";
        }

        /// <summary>
        /// Converts a `SquadCreatedModel` entity into a `SquadInfoModel` for storage.
        /// </summary>
        /// <param name="entity">The `SquadCreatedModel` entity to convert.</param>
        /// <returns>A new instance of `SquadInfoModel`.</returns>
        public static SquadCreatedEventModel FromEntity(SquadCreatedInfo entity)
        {
            return new SquadCreatedEventModel
            {
                EosId = entity.CreatorIds.EosId,
                SteamId = entity.CreatorIds.SteamId,
                PlayerName = entity.PlayerName,
                SquadId = entity.SquadId,
                SquadName = entity.SquadName,
                TeamName = entity.TeamName,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Converts the `SquadCreatedModel` back into a `SquadCreatedInfo` entity.
        /// </summary>
        /// <returns>A `SquadCreatedInfo` entity.</returns>
        public SquadCreatedInfo ToEntity()
        {
            return new SquadCreatedInfo
            {
                PlayerName = PlayerName,
                SquadId = SquadId,
                SquadName = SquadName,
                TeamName = TeamName,
                CreatorIds = new CreatorOnlineIds(EosId, SteamId)
            };
        }
    }
}
