using Microsoft.Extensions.DependencyInjection;
using SquadNET.LogManagement;
using SquadNET.Rcon;
using System.Runtime.CompilerServices;

namespace SquadNET.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSquadApplication(this IServiceCollection services)
        {
            services.AddLogManagement();
            services.AddRconServices();

            return services;
        }
    }
}
