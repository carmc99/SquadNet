using SquadNET.Core.Squad.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Models
{
    public class LayerModel
    {
        public string Name { get; set; }
        public string Classname { get; set; }
        public string LayerId { get; set; }
        public MapInfo Map { get; set; }
        public string Gamemode { get; set; }
        public string GamemodeType { get; set; }
        public string Version { get; set; }
        public string Size { get; set; }
        public string SizeType { get; set; }
        public int NumberOfCapturePoints { get; set; }
        public LightingInfo Lighting { get; set; }
        public List<TeamInfo> Teams { get; set; } = new List<TeamInfo>();
    }
}
