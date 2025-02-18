using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SquadNET.Core
{
    public static class RegexPatternHelper
    {
        public static Regex GetRegex<T>()
        {
            RegexPatternAttribute attribute = typeof(T).GetCustomAttribute<RegexPatternAttribute>();

            if (attribute == null)
            {
                throw new InvalidOperationException($"La clase {typeof(T).Name} no tiene un atributo RegexPatternAttribute.");
            }

            return new Regex(attribute.Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}
