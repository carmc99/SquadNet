﻿using Microsoft.Extensions.DependencyInjection;
using SquadNET.Core.Squad.Models;
using SquadNET.Core.Squad.Parsers;
using SquadNET.Core;
using SquadNET.Core.Squad.Entities;
using Squadmania.Squad.Rcon.Parsers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSquadParsers(this IServiceCollection services)
    {
        services.AddSingleton<ICommandParser<ListPlayerModel>, ListPlayersParser>();
        services.AddSingleton<ICommandParser<List<CommandInfo>>, ListCommandsParser>();
        services.AddSingleton<ICommandParser<List<SquadInfo>>, ListSquadsParser>();
        services.AddSingleton<ICommandParser<List<TeamInfo>>, ListTeamsParser>();
        services.AddSingleton<ICommandParser<CurrentMapInfo>, ShowCurrentMapParser>();
        services.AddSingleton<ICommandParser<NextMapInfo>, ShowNextMapParser>();
        services.AddSingleton<ICommandParser<ChatMessageInfo>, ChatMessageParser>();
        services.AddSingleton<ICommandParser<SquadCreatedInfo>, SquadCreatedMessageParser>();
        services.AddSingleton<ICommandParser<List<LayerInfo>>, ListLayersParser>();
        services.AddSingleton<ICommandParser<List<LevelInfo>>, ListLevelsParser>();
        return services;
    }

    public static IServiceCollection AddSquad(this IServiceCollection services)
    {
        services.AddSquadParsers();
        return services;
    }
}
