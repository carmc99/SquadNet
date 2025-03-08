// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^([A-Za-z]+)\s+(.*)\s+(\([A-Za-z\s\.\-]+\))$")]
    public class CommandInfo
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string ParameterDescription { get; set; }
    }
}