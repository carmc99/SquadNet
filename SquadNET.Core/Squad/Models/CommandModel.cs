// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>

namespace SquadNET.Core.Squad.Models
{
    [RegexPattern(@"^([\w\.\-]+)(?:\s+([^\(]+))?\s+(\(.*\))$")]
    public class CommandModel
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string ParameterDescription { get; set; }
    }
}