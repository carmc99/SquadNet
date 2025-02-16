using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Domain.Entities
{
    public class Team
    {
        public string Faction { get; set; }
        public string Name { get; set; }
        public int Tickets { get; set; }
        public string Commander { get; set; }
        public List<Vehicle> Vehicles { get; set; } = [];
        public int NumberOfTanks { get; set; }
        public int NumberOfHelicopters { get; set; }
    }
}
