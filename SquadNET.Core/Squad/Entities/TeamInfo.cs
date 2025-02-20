using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core.Squad.Entities
{
    [RegexPattern(@"^Team ID: ([0-9]+) \((.+)\)$")]
    public class TeamInfo
    {
        public TeamId Id { get; set; }
        public string Name { get; set; }

        public bool Equals(
            TeamInfo other
        )
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Name == other.Name;
        }

        public override bool Equals(
            object obj
        )
        {
            return ReferenceEquals(this, obj) || obj is TeamInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Id, Name);
        }
    }
}
