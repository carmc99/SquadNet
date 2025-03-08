// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Events.Models
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogWorld: Bringing World \/([A-z]+)\/(?:Maps\/)?([A-z0-9-]+)\/(?:.+\/)?([A-z0-9-]+)(?:\.[A-z0-9-]+)")]
    public class NewGameEventModel
    {
        public string ChainID { get; set; }
        public string DLC { get; set; }
        public string LayerClassname { get; set; }
        public string MapClassname { get; set; }
        public string Time { get; set; }
    }
}