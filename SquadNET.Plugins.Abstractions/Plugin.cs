// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using SquadNET.Core.Squad.Events;

namespace SquadNET.Plugins.Abstractions
{
    public abstract class Plugin : IPlugin
    {
        public abstract string Name { get; }

        /// <inheritdoc />
        public virtual void Initialize()
        { }

        /// <inheritdoc />
        public virtual void OnEventRaised(string eventName, ISquadEventData eventData)
        {
            // Por defecto, no hace nada.
        }

        /// <inheritdoc />
        public virtual void Shutdown()
        { }
    }
}