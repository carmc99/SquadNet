using System;

namespace SquadNET.Plugins.Abstractions
{
    public abstract class Plugin : IPlugin
    {
        public abstract string Name { get; }

        /// <summary>
        /// Implementación base de inicialización. 
        /// Los plugins pueden sobreescribir este método si es necesario.
        /// </summary>
        public virtual void Initialize()
        {
            Console.WriteLine($"[{Name}] Plugin inicializado.");
        }

        /// <summary>
        /// Evento que recibe el nombre del evento y los datos asociados.
        /// Cada plugin puede sobreescribir este método para procesar los eventos deseados.
        /// </summary>
        public virtual void OnEventRaised(string eventName, object eventData)
        {
            // Por defecto, no hace nada. 
            // Evita lanzar excepción para que no obligue a su re-implementación inmediata.
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