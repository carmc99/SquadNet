// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
// <copyright company="SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.Extensions.DependencyInjection;
using SquadNET.Core;
using SquadNET.Core.Squad.Commands;
using SquadNET.Core.Squad.Entities;
using SquadNET.Core.Squad.Events.Models;
using SquadNET.Core.Squad.Models;
using SquadNET.Core.Squad.Parsers;
using SquadNET.Rcon;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSquad(this IServiceCollection services)
    {
        services.AddSquadParsers();
        services.AddSingleton<Command<SquadCommand>, SquadCommandTemplate>();
        return services;
    }

    public static IServiceCollection AddSquadParsers(this IServiceCollection services)
    {
        services.AddSingleton<IParser<ListPlayerModel>, ListPlayersParser>();
        services.AddSingleton<IParser<List<CommandModel>>, ListCommandsParser>();
        services.AddSingleton<IParser<List<SquadModel>>, ListSquadsParser>();
        services.AddSingleton<IParser<List<TeamModel>>, ListTeamsParser>();
        services.AddSingleton<IParser<CurrentMapModel>, ShowCurrentMapParser>();
        services.AddSingleton<IParser<NextMapModel>, ShowNextMapParser>();
        services.AddSingleton<IParser<ChatMessageEventModel>, ChatMessageParser>();
        services.AddSingleton<IParser<SquadCreatedEventModel>, SquadCreatedMessageParser>();
        services.AddSingleton<IParser<List<LayerModel>>, ListLayersParser>();
        services.AddSingleton<IParser<List<LevelModel>>, ListLevelsParser>();

        services.AddSingleton<IParser<RoundEndedEventModel>, RoundEndedParser>();
        services.AddSingleton<IParser<RoundTicketsEventModel>, RoundTicketsParser>();
        services.AddSingleton<IParser<RoundWinnerEventModel>, RoundWinnerParser>();
        services.AddSingleton<IParser<ServerTickRateEventModel>, ServerTickRateParser>();
        services.AddSingleton<IParser<PlayerDamagedEventModel>, PlayerDamagedParser>();
        services.AddSingleton<IParser<PlayerJoinSucceededEventModel>, PlayerJoinSucceededParser>();
        services.AddSingleton<IParser<PlayerPossessEventModel>, PlayerPossessParser>();
        services.AddSingleton<IParser<PlayerRevivedEventModel>, PlayerRevivedParser>();
        services.AddSingleton<IParser<AdminBroadcastEventModel>, AdminBroadcastParser>();
        services.AddSingleton<IParser<ServerInformationModel>, ServerInformationParser>();
        services.AddSingleton<IParser<PlayerUnPossessEventModel>, PlayerUnPossessParser>();

        return services;
    }
}