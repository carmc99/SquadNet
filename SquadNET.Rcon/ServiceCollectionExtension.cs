// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.DependencyInjection;
using SquadNET.Core;

namespace SquadNET.Rcon
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRconServices(this IServiceCollection services)
        {
            services.AddCore();
            services.AddScoped<IRconService, SquadRcon>();
            return services;
        }
    }
}