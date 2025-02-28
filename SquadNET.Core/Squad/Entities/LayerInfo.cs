using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    public class LayerInfo
    {
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is LayerInfo other && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
