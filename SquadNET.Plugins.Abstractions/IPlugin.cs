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
        /// Plugin name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Shuts down the plugin.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Handles an event triggered by the application.
        /// </summary>
        /// <param name="eventName">Name of the event (e.g., "CHAT_MESSAGE").</param>
        /// <param name="eventData">Object containing relevant event information.</param>
        void OnEventRaised(string eventName, IEventData eventData);
    }
}
