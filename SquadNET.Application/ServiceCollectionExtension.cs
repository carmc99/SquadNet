// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SquadNET.Application.Squad.Map.Repositories.EF;
using SquadNET.Application.Squad.Player.Repositories.EF;
using SquadNET.Application.Squad.Server.Repositories.EF;
using SquadNET.Application.Squad.Team.Repositories.EF;
using SquadNET.LogManagement;
using SquadNET.Rcon;
using System.Reflection;

namespace SquadNET.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext<MapDbContext>(services, configuration);
            AddDbContext<PlayerDbContext>(services, configuration);
            AddDbContext<TeamDbContext>(services, configuration);
            AddDbContext<ServerDbContext>(services, configuration);

            services.AddServerRepository();
            services.AddPlayerRepository();
            services.AddTeamRepository();
            services.AddMapRepository();

            return services;
        }

        public static IServiceCollection AddSquadApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogManagement();
            services.AddRconServices();
            services.AddRepositories(configuration);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }

        private static void AddDbContext<T>(IServiceCollection services, IConfiguration configuration) where T : DbContextBase
        {
            services.AddDbContextFactory<T>(options =>
            {
                DbContextBase.ConfigureDatabaseProvider(options, configuration);
            });
        }
    }
}