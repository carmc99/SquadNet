using Microsoft.Extensions.DependencyInjection;

namespace SquadNET.Rcon
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRcon(this IServiceCollection services)
        {
            return services;
        }
    }
}
