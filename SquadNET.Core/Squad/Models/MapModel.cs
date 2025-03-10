// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Models
{
    [RegexPattern(@"^Current level is ([^,]+), layer is ([^,]+)")]
    public class CurrentMapModel
    {
        public string Level { get; set; }
        public string Name { get; set; }
    }

    public class MapModel
    {
        public CurrentMapModel CurrentMap { get; set; }
        public NextMapModel NextMap { get; set; }
    }

    [RegexPattern(@"^Next level is ([^,]+), layer is ([^,]+)")]
    public class NextMapModel
    {
        public string Level { get; set; }
        public string Name { get; set; }
    }
}