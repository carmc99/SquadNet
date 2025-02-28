using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SquadNET.LogManagement;
using SquadNET.Rcon;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SquadNET.Application
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSquadApplication(this IServiceCollection services)
        {
            services.AddLogManagement();
            services.AddRconServices();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
