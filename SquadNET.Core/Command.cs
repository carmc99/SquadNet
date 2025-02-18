using SquadNET.Rcon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core
{
    public abstract class Command<T>
    {
        protected readonly Dictionary<T, string> CommandTemplates = [];

        public string GetFormattedCommand(T command, params object[] args)
        {
            if (!CommandTemplates.ContainsKey(command))
            {
                throw new InvalidOperationException($"El comando '{command}' no está definido en la plantilla.");
            }
            return string.Format(CommandTemplates[command], args);
        }
    }
}
