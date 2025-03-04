// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class RegexPatternAttribute : Attribute
    {
        public RegexPatternAttribute(string pattern)
        {
            Pattern = pattern;
        }

        public string Pattern { get; }
    }
}