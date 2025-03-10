// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"\[.*?\]LogNet: UChannel::Close: Sending CloseBunch\. ChIndex == \d+\. Name: \[UChannel\] ChIndex: \d+, Closing: \d+ \[UNetConnection\] RemoteAddr: ([0-9.]+:\d+), Name: ([^,]+), Driver: ([^,]+), IsServer: (YES|NO), PC: ([^,]+), Owner: ([^,]+), UniqueId: RedpointEOS:([0-9a-f]+)")]
    public class PlayerDisconnectedEventModel : ISquadEventData
    {
        public string ConnectionName { get; set; }
        public string Driver { get; set; }
        public string EosId { get; set; }
        public bool IsServer { get; set; }
        public string Owner { get; set; }
        public string PlayerController { get; set; }
        public string RemoteAddress { get; set; }

        public static PlayerDisconnectedEventModel FromParsedData(
            string remoteAddress, string connectionName, string driver,
            string isServer, string playerController, string owner, string eosId)
        {
            return new PlayerDisconnectedEventModel
            {
                RemoteAddress = remoteAddress,
                ConnectionName = connectionName,
                Driver = driver,
                IsServer = isServer == "YES",
                PlayerController = playerController,
                Owner = owner,
                EosId = eosId
            };
        }
    }
}