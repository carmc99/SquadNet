using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Domain
{
    // Atributo para asociar un patrón regex a una clase
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class RegexPatternAttribute : Attribute
    {
        public string Pattern { get; }
        public RegexPatternAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}
