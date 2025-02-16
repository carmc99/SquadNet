using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace SquadNET.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSquadApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
