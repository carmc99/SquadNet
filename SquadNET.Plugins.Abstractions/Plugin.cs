using SquadNET.Core.Squad.Models;
using System;

namespace SquadNET.Plugins.Abstractions
{
    public abstract class Plugin : IPlugin
    {
        public abstract string Name { get; }

        /// <inheritdoc />
        public virtual void Initialize() { }

        /// <inheritdoc />
        public virtual void OnEventRaised(string eventName, IEventData eventData)
        {
            // Por defecto, no hace nada. 
            // Evita lanzar excepción para que no obligue a su re-implementación inmediata.
        }

        /// <inheritdoc />
        public virtual void Shutdown() { }
    }
}