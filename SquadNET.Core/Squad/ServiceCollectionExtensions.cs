using Microsoft.Extensions.DependencyInjection;
using SquadNET.Core.Squad.Models;
using SquadNET.Core.Squad.Parsers;
using SquadNET.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSquadParsers(this IServiceCollection services)
    {
        services.AddSingleton<ICommandParser<ListPlayerModel>, ListPlayersParser>();
        return services;
    }

    public static IServiceCollection AddSquad(this IServiceCollection services)
    {
        services.AddSquadParsers();
        return services;
    }
}
