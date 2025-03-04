// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using System.Reflection;
using System.Text.RegularExpressions;

namespace SquadNET.Core
{
    public static class RegexPatternHelper
    {
        public static Regex GetRegex<T>()
        {
            RegexPatternAttribute attribute = typeof(T).GetCustomAttribute<RegexPatternAttribute>();

            if (attribute == null)
            {
                throw new InvalidOperationException($"The class {typeof(T).Name} does not have a RegexPatternAttribute.");
            }

            return new Regex(attribute.Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}