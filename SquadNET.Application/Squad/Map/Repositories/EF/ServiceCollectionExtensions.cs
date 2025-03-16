// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.DependencyInjection;

namespace SquadNET.Application.Squad.Map.Repositories.EF
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMapRepository(this IServiceCollection services)
        {
            services.AddScoped<IMapRepository, MapRepository>();

            return services;
        }
    }
}