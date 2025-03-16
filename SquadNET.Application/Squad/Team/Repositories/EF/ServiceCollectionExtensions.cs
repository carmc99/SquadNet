// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.DependencyInjection;

namespace SquadNET.Application.Squad.Team.Repositories.EF
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTeamRepository(this IServiceCollection services)
        {
            services.AddScoped<ITeamRepository, TeamRepository>();

            return services;
        }
    }
}