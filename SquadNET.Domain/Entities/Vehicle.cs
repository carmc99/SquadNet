using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Domain.Entities
{
    public class Vehicle
    {
        public string Name { get; set; }
        public string Classname { get; set; }
        public int Count { get; set; }
        public double SpawnDelay { get; set; }
        public double RespawnDelay { get; set; }
    }
}
