// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: ADMIN COMMAND: Message broadcasted <(.+)> from (.+)")]
    public class AdminBroadcastEventModel : ISquadEventData
    {
        public string ChainID { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
        public string Time { get; set; }
    }
}