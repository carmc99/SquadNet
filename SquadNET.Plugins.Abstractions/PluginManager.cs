﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        }

        /// <summary>
        /// Inicializa los plugins registrados en el contenedor de inyección de dependencias.
        /// </summary>
        public void InitializePlugins()
        {
            Logger.LogInformation("[PluginManager] Inicializando plugins...");

            IEnumerable<IPlugin> registeredPlugins = ServiceProvider.GetServices<IPlugin>();

            foreach (IPlugin plugin in registeredPlugins)
            {
                try
                {
                    plugin.Initialize();
                    Plugins.Add(plugin);
                    Logger.LogInformation("[PluginManager] Plugin inicializado: {PluginName}", plugin.Name);
                }
                catch (Exception ex)
                {
                    Logger.LogError("[PluginManager] Error al inicializar el plugin {PluginName}: {Message}", plugin.Name, ex.Message);
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
                    Logger.LogError("[PluginManager] Error al apagar el plugin {PluginName}: {Message}", plugin.Name, ex.Message);
                }
            }

            Plugins.Clear();
        }
    }
}
