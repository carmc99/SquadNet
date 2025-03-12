// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SquadNET.Application.Squad.Server.Repositories.EF;

namespace SquadNET.Application.Squad.Player.Repositories.EF
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlayerRepository(this IServiceCollection services)
        {
            //TODO: only in dev, define database later
            services.AddDbContextFactory<PlayerDbContext>(options =>
            {
                options.UseInMemoryDatabase("PlayerDatabase");
            });
            services.AddScoped<IPlayerRepository, PlayerRepository>();

            return services;
        }
    }
}