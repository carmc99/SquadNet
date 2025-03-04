// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Events;

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
        /// Handles an event triggered by the application.
        /// </summary>
        /// <param name="eventName">Name of the event (e.g., "CHAT_MESSAGE").</param>
        /// <param name="eventData">Object containing relevant event information.</param>
        void OnEventRaised(string eventName, ISquadEventData eventData);

        /// <summary>
        /// Shuts down the plugin.
        /// </summary>
        void Shutdown();
    }
}