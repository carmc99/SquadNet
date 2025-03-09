// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad.Models
{
    public class LevelModel
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LevelModel other && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}