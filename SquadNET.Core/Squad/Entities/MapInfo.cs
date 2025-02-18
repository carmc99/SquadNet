using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{

    [RegexPattern(@"^Next level is (.*), layer is (.*)$")]
    [RegexPattern(@"^Current level is (.*), layer is (.*)$")]
    public class MapInfo
    {
        public string Name { get; set; }
        public string Level { get; set; }
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
