// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.DependencyInjection;
using SquadNET.Application.Services;
using System.Reflection;

namespace SquadNET.Plugins.Abstractions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPlugins(this IServiceCollection services, string pluginsPath)
        {
            if (string.IsNullOrWhiteSpace(pluginsPath))
            {
                throw new ArgumentException("Plugins path cannot be null or empty", nameof(pluginsPath));
            }

            if (!Directory.Exists(pluginsPath))
            {
                Directory.CreateDirectory(pluginsPath);
            }

            services.AddSingleton<PluginManager>();

            IEnumerable<Assembly> pluginAssemblies = Directory.GetFiles(pluginsPath, "*.dll", SearchOption.TopDirectoryOnly)
                .Select(Assembly.LoadFrom);

            foreach (Assembly assembly in pluginAssemblies)
            {
                IEnumerable<Type> pluginTypes = assembly.GetTypes()
                    .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                foreach (Type type in pluginTypes)
                {
                    services.AddSingleton(typeof(IPlugin), type);
                }
            }

            return services;
        }
    }
}