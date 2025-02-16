using Microsoft.Extensions.DependencyInjection;

namespace SquadNET.Persistence
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services;
        }
    }
}
