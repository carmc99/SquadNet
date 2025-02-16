using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Domain
{
    public static class RegexPatternHelper
    {
        public static string GetRegexPattern<T>()
        {
            RegexPatternAttribute attribute = typeof(T).GetCustomAttribute<RegexPatternAttribute>();

            return attribute == null
                ? throw new InvalidOperationException($"La clase {typeof(T).Name} no tiene un atributo RegexPatternAttribute.")
                : attribute.Pattern;
        }
    }
}
