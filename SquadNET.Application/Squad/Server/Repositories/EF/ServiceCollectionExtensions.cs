// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SquadNET.Application.Squad.Server.Repositories.EF
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServerRepository(this IServiceCollection services)
        {
            //TODO: only in dev, define database later
            services.AddDbContextFactory<ServerDbContext>(options =>
            {
                options.UseInMemoryDatabase("ServerDatabase");
            });
            services.AddScoped<IServerInfoRepository, ServerInfoRepository>();

            return services;
        }
    }
}