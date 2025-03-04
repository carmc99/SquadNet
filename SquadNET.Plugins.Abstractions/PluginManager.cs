// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SquadNET.Core.Squad.Events;
using SquadNET.Plugins.Abstractions;

namespace SquadNET.Application.Services
{
    public class PluginManager
    {
        private readonly ILogger<PluginManager> Logger;
        private readonly List<IPlugin> Plugins = [];
        private readonly IServiceProvider ServiceProvider;

        public PluginManager(IServiceProvider serviceProvider, ILogger<PluginManager> logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;

            LoadPlugins();
        }

        /// <summary>
        /// Triggers an event to be consumed by supported plugins.
        /// </summary>
        public void EmitEvent(string eventName, ISquadEventData eventData)
        {
            foreach (IPlugin plugin in Plugins)
            {
                try
                {
                    plugin.OnEventRaised(eventName, eventData);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "[PluginManager] Error emitting event {EventName} to plugin {PluginName}",
                        eventName, plugin.Name);
                }
            }
        }

        /// <summary>
        /// Shuts down and clears the loaded plugins.
        /// </summary>
        public void ShutdownPlugins()
        {
            Logger.LogInformation("[PluginManager] Shutting down plugins...");

            foreach (IPlugin plugin in Plugins)
            {
                try
                {
                    plugin.Shutdown();
                    Logger.LogInformation("[PluginManager] Plugin shut down: {PluginName}", plugin.Name);
                }
                catch (Exception ex)
                {
                    Logger.LogError("[PluginManager] Error shutting down plugin {PluginName}: {Message}",
                        plugin.Name, ex.Message);
                }
            }

            Plugins.Clear();
        }

        /// <summary>
        /// Loads the plugins registered in the dependency injection container.
        /// </summary>
        private void LoadPlugins()
        {
            Logger.LogInformation("[PluginManager] Loading plugins...");

            IEnumerable<IPlugin> registeredPlugins = ServiceProvider.GetServices<IPlugin>();
            foreach (IPlugin plugin in registeredPlugins)
            {
                try
                {
                    plugin.Initialize();
                    Plugins.Add(plugin);
                    Logger.LogInformation("[PluginManager] Plugin loaded: {PluginName}", plugin.Name);
                }
                catch (Exception ex)
                {
                    Logger.LogError("[PluginManager] Error loading plugin {PluginName}: {Message}",
                        plugin.Name, ex.Message);
                }
            }
        }
    }
}