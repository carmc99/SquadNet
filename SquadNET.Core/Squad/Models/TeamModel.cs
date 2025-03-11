// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Models
{
    [RegexPattern(@"^Team ID: ([0-9]+) \((.+)\)$")]
    public class TeamModel
    {
        public TeamType Id { get; set; }
        public string Name { get; set; }

        public bool Equals(
            TeamModel other
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
            return ReferenceEquals(this, obj) || obj is TeamModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Id, Name);
        }
    }
}