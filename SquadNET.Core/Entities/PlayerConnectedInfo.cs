using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Entities
{
    [RegexPattern(@"^\[([0-9.:-]+)]\[([ 0-9]*)]LogSquad: PostLogin: NewPlayer: BP_PlayerController_C .+PersistentLevel\.([^\s]+) \(IP: ([\d.]+) \| Online IDs:([^)|]+)\)")]
    public class PlayerConnectedInfo
    {
        public RawDataInfo Raw { get; set; }
        public string Time { get; set; }
        public int ChainID { get; set; }
        public string PlayerController { get; set; }
        public string IP { get; set; }
        /// <summary>
        /// Propiedades adicionales derivadas de los IDs (por ejemplo, eosID, steam, etc.)
        /// </summary>
        public Dictionary<string, string> AdditionalIDs { get; set; } = new();
    }
}
