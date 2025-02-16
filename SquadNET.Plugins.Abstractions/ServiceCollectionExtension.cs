using Microsoft.Extensions.DependencyInjection;

namespace SquadNET.Plugins.Abstractions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPlugins(this IServiceCollection services)
        {
            return services;
        }
    }
}
