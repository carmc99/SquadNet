using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Plugins.Abstractions
{
    public abstract class Plugin : IPlugin
    {
        public abstract string Name { get; }

        /// <summary>
        /// Implementación base de inicialización. Los plugins pueden sobreescribir este método si es necesario.
        /// </summary>
        public virtual void Initialize()
        {
            Console.WriteLine($"[{Name}] Plugin inicializado.");
        }

        /// <summary>
        /// Implementación base de apagado. Se puede sobreescribir si es necesario.
        /// </summary>
        public virtual void Shutdown()
        {
            Console.WriteLine($"[{Name}] Plugin apagado.");
        }
    }
}
