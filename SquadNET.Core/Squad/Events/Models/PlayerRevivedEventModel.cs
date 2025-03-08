// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Entities;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: (.+) \(Online IDs:([^)]+)\) has revived (.+) \(Online IDs:([^)]+)\)\.")]
    public class PlayerRevivedEventModel : ISquadEventData
    {
        public string ChainID { get; set; }
        public CreatorOnlineIds ReviverIds { get; set; }
        public string ReviverName { get; set; }
        public string Time { get; set; }
        public CreatorOnlineIds VictimIds { get; set; }
        public string VictimName { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="PlayerRevivedEventModel"/> from parsed log data.
        /// </summary>
        public static PlayerRevivedEventModel FromParsedData(string time, string chainID, string reviverName, string reviverOnlineIds,
                                                             string victimName, string victimOnlineIds)
        {
            return new PlayerRevivedEventModel
            {
                Time = time,
                ChainID = chainID,
                ReviverName = reviverName,
                ReviverIds = CreatorOnlineIds.FromString(reviverOnlineIds),
                VictimName = victimName,
                VictimIds = CreatorOnlineIds.FromString(victimOnlineIds)
            };
        }
    }
}