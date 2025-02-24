using Microsoft.Extensions.DependencyInjection;
using SquadNET.Core.Squad.Models;
using SquadNET.Core.Squad.Parsers;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSquadParsers(this IServiceCollection services)
    {
        services.AddSingleton<IParser<ListPlayerModel>, ListPlayersParser>();
        services.AddSingleton<IParser<List<CommandInfo>>, ListCommandsParser>();
        services.AddSingleton<IParser<List<SquadInfo>>, ListSquadsParser>();
        services.AddSingleton<IParser<List<TeamInfo>>, ListTeamsParser>();
        services.AddSingleton<IParser<CurrentMapInfo>, ShowCurrentMapParser>();
        services.AddSingleton<IParser<NextMapInfo>, ShowNextMapParser>();
        services.AddSingleton<IParser<ChatMessageInfo>, ChatMessageParser>();
        services.AddSingleton<IParser<SquadCreatedInfo>, SquadCreatedMessageParser>();
        services.AddSingleton<IParser<List<LayerInfo>>, ListLayersParser>();
        services.AddSingleton<IParser<List<LevelInfo>>, ListLevelsParser>();
        return services;
    }

    public static IServiceCollection AddSquad(this IServiceCollection services)
    {
        services.AddSquadParsers();
        return services;
    }
}
