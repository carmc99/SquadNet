// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SquadNET.Application.Squad.Server.Repositories.EF;

namespace SquadNET.Application.Squad.Map.Repositories.EF
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMapRepository(this IServiceCollection services)
        {
            //TODO: only in dev, define database later
            services.AddDbContextFactory<MapDbContext>(options =>
            {
                options.UseInMemoryDatabase("MapDatabase");
            });
            services.AddScoped<IMapRepository, MapRepository>();

            return services;
        }
    }
}