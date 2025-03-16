// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.DependencyInjection;

namespace SquadNET.Application.Squad.Player.Repositories.EF
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlayerRepository(this IServiceCollection services)
        {
            services.AddScoped<IPlayerRepository, PlayerRepository>();

            return services;
        }
    }
}