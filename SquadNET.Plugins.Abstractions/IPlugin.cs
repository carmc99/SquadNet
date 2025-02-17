using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Plugins.Abstractions
{
    public interface IPlugin
    {
        /// <summary>
        /// Nombre del plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Método de inicialización del plugin.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Método de apagado del plugin.
        /// </summary>
        void Shutdown();
    }
}
