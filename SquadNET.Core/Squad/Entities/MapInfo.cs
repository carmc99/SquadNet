using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    public class MapInfo
    {
        public NextLevelInfo NextMap { get; set; }
        public CurrentLevelInfo CurrentMap { get; set; }
    }

    [RegexPattern(@"^Next level is (.*), layer is (.*)$")]
    public class NextLevelInfo
    {
        public string Level { get; set; }
        public string Name { get; set; }
    }

    [RegexPattern(@"^Current level is (.*), layer is (.*)$")]
    public class CurrentLevelInfo
    {
        public string Level { get; set; }
        public string Name { get; set; }
    }
}
