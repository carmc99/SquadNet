// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

using SquadNET.Core.Squad.Models;

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"\[.*?\]LogSquad: PostLogin: NewPlayer: (.*?) \(IP: ([0-9.]+) \| Online IDs: EOS: ([0-9a-f]+) steam: (\d+)\)")]
    public class PlayerConnectedEventModel : ISquadEventData
    {
        public CreatorOnlineModel CreatorIds { get; set; }
        public string IP { get; set; }
        public string Name { get; set; }

        public static PlayerConnectedEventModel FromParsedData(string name, string ip, string eosId, ulong steamId)
        {
            return new PlayerConnectedEventModel
            {
                Name = name,
                IP = ip,
                CreatorIds = new CreatorOnlineModel(eosId, steamId)
            };
        }
    }
}