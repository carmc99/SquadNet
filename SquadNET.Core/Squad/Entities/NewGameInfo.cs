using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogWorld: Bringing World \/([A-z]+)\/(?:Maps\/)?([A-z0-9-]+)\/(?:.+\/)?([A-z0-9-]+)(?:\.[A-z0-9-]+)")]
    public class NewGameInfo
    {
        public RawDataInfo Raw { get; set; }
        public string Time { get; set; }
        public string ChainID { get; set; }
        public string DLC { get; set; }
        public string MapClassname { get; set; }
        public string LayerClassname { get; set; }
    }
}
