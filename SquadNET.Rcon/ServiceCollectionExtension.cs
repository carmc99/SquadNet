using Microsoft.Extensions.DependencyInjection;
using SquadNET.Core;
using SquadNET.Core.Squad.Commands;

namespace SquadNET.Rcon
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRconServices(this IServiceCollection services)
        {
            services.AddCore();
            services.AddScoped<IRconService, SquadRcon>();
            services.AddSingleton<Command<SquadCommand>, SquadCommandTemplate>();
            return services;
        }
    }
}
