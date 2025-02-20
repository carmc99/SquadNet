using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    public class MapInfo
    {
        public NextMapInfo NextMap { get; set; }
        public CurrentMapInfo CurrentMap { get; set; }
    }

    [RegexPattern(@"^Next level is (.*), layer is (.*)$")]
    public class NextMapInfo
    {
        public string Level { get; set; }
        public string Name { get; set; }
    }

    [RegexPattern(@"^Current level is (.*), layer is (.*)$")]
    public class CurrentMapInfo
    {
        public string Level { get; set; }
        public string Name { get; set; }
    }
}
