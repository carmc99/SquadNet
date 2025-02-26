using Microsoft.Extensions.DependencyInjection;
using SquadNET.Core.Squad.Models;
using SquadNET.Core.Squad.Parsers;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events.Models;

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

        services.AddSingleton<IParser<RoundEndedEventModel>, RoundEndedParser>();
        services.AddSingleton<IParser<RoundTicketsEventModel>, RoundTicketsParser>();
        services.AddSingleton<IParser<RoundWinnerEventModel>, RoundWinnerParser>();
        services.AddSingleton<IParser<ServerTickRateEventModel>, ServerTickRateParser>();
        services.AddSingleton<IParser<PlayerDamagedEventModel>, PlayerDamagedParser>();
        services.AddSingleton<IParser<PlayerJoinSucceededEventModel>, PlayerJoinSucceededParser>();
        services.AddSingleton<IParser<PlayerPossessEventModel>, PlayerPossessParser>();
        services.AddSingleton<IParser<PlayerRevivedEventModel>, PlayerRevivedParser>();
        services.AddSingleton<IParser<AdminBroadcastEventModel>, AdminBroadcastParser>();

        return services;
    }

    public static IServiceCollection AddSquad(this IServiceCollection services)
    {
        services.AddSquadParsers();
        return services;
    }
}
