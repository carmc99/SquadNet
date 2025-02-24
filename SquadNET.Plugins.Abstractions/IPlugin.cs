using SquadNET.Core.Squad.Models;
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

        /// <summary>
        /// Maneja un evento disparado por la aplicación.
        /// </summary>
        /// <param name="eventName">Nombre del evento (por ejemplo, "CHAT_MESSAGE").</param>
        /// <param name="eventData">Objeto con la información relevante del evento.</param>
        void OnEventRaised(string eventName, IEventData eventData);
    }
}
