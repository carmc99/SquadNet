// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SquadNET.Application.Squad.Map.Queries;
using SquadNET.Application.Squad.Player.Queries;
using SquadNET.Application.Squad.Server.Queries;
using SquadNET.Application.Squad.Server.Repositories.EF;
using SquadNET.Application.Squad.Team.Queries;
using SquadNET.Core.Squad.Models;

namespace SquadNET.SquadMonitoringService
{
    public class SquadDataUpdateService : BackgroundService
    {
        private readonly ILogger Logger;
        private readonly IMediator Mediator;
        private readonly IServerInfoRepository ServerInfoRepository;
        private readonly TimeSpan UpdateInterval = TimeSpan.FromSeconds(30);

        public SquadDataUpdateService(
            IMediator mediator,
            ILogger<SquadDataUpdateService> logger,
            IServerInfoRepository serverInfoRepository)
        {
            ServerInfoRepository = serverInfoRepository;
            Mediator = mediator;
            Logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Squad Data Update Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateServerInformationAsync();
                    await UpdatePlayerListAsync();
                    await UpdateSquadListAsync();
                    await UpdateLayerInformationAsync();

                    Logger.LogInformation("Squad data updated successfully.");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error updating Squad data.");
                }

                await Task.Delay(UpdateInterval, stoppingToken);
            }

            Logger.LogInformation("Squad Data Update Service is stopping.");
        }

        private async Task UpdateLayerInformationAsync()
        {
            List<LayerModel> layers = await Mediator.Send(new ListLayersQuery.Request());
            Logger.LogInformation("Layer list updated: {LayerCount} layers", layers?.Count);
        }

        private async Task UpdatePlayerListAsync()
        {
            ListPlayerModel players = await Mediator.Send(new ListPlayersQuery.Request());
            Logger.LogInformation("Player list updated: {PlayerCount} players", players?.ActivePlayers.Count);
        }

        private async Task UpdateServerInformationAsync()
        {
            ServerInformationModel serverInfo = await Mediator.Send(new ServerInformationQuery.Request());
            await ServerInfoRepository.Store(serverInfo);
            Logger.LogInformation("Server information updated: {ServerName}", serverInfo?.ServerName);
        }

        private async Task UpdateSquadListAsync()
        {
            List<TeamModel> squads = await Mediator.Send(new ListTeamsQuery.Request());
            Logger.LogInformation("Squad list updated: {SquadCount} squads", squads?.Count);
        }
    }
}