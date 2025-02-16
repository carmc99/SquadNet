using Microsoft.Extensions.DependencyInjection;

namespace SquadNET.LogManagement
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLogManagement(this IServiceCollection services)
        {
            return services;
        }
    }
}
