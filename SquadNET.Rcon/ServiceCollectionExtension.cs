using Microsoft.Extensions.DependencyInjection;

namespace SquadNET.Rcon
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRconServices(this IServiceCollection services)
        {
            services.AddSingleton<IRconService, SquadRcon>();
            return services;
        }
    }
}
