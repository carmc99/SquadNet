using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SquadNET.Core.Squad.Models;
using SquadNET.Plugins.Abstractions;

namespace SquadNET.Application.Services
{
    public class PluginManager
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger<PluginManager> Logger;
        private readonly List<IPlugin> Plugins = [];

        public PluginManager(IServiceProvider serviceProvider, ILogger<PluginManager> logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;

            LoadPlugins();
        }

        /// <summary>
        /// Carga los plugins registrados en el contenedor de inyección de dependencias.
        /// </summary>
        private void LoadPlugins()
        {
            Logger.LogInformation("[PluginManager] Cargando plugins...");

            IEnumerable<IPlugin> registeredPlugins = ServiceProvider.GetServices<IPlugin>();
            foreach (IPlugin plugin in registeredPlugins)
            {
                try
                {
                    plugin.Initialize();
                    Plugins.Add(plugin);
                    Logger.LogInformation("[PluginManager] Plugin cargado: {PluginName}", plugin.Name);
                }
                catch (Exception ex)
                {
                    Logger.LogError("[PluginManager] Error al cargar el plugin {PluginName}: {Message}",
                        plugin.Name, ex.Message);
                }
            }
        }

        /// <summary>
        /// Apaga y limpia los plugins cargados.
        /// </summary>
        public void ShutdownPlugins()
        {
            Logger.LogInformation("[PluginManager] Apagando plugins...");

            foreach (IPlugin plugin in Plugins)
            {
                try
                {
                    plugin.Shutdown();
                    Logger.LogInformation("[PluginManager] Plugin apagado: {PluginName}", plugin.Name);
                }
                catch (Exception ex)
                {
                    Logger.LogError("[PluginManager] Error al apagar el plugin {PluginName}: {Message}",
                        plugin.Name, ex.Message);
                }
            }

            Plugins.Clear();
        }

        /// <summary>
        /// Dispara un evento para que lo consuman los plugins que lo soporten.
        /// </summary>
        public void EmitEvent(string eventName, IEventData eventData)
        {
            foreach (IPlugin plugin in Plugins)
            {
                try
                {
                    plugin.OnEventRaised(eventName, eventData);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "[PluginManager] Error emitiendo evento {EventName} a plugin {PluginName}",
                        eventName, plugin.Name);
                }
            }
        }
    }
}
