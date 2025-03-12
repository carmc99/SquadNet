// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using SquadNET.Application.Squad.Server.Repositories.EF;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Player.Repositories.EF
{
    internal class PlayerRepository : IPlayerRepository
    {
        private readonly PlayerDbContext Context;

        public PlayerRepository(IDbContextFactory<PlayerDbContext> contextFactory)
        {
            Context = contextFactory.CreateDbContext();
        }

        public async Task Store(ListPlayerModel model)
        {
            if (model.ActivePlayers != null)
            {
                foreach (PlayerConnectedModel player in model.ActivePlayers)
                {
                    PlayerConnectedModel existingPlayer = await Context.ActivePlayers
                        .FirstOrDefaultAsync(p => p.Id == player.Id);

                    if (existingPlayer != null)
                    {
                        Context.Entry(existingPlayer).CurrentValues.SetValues(player);
                    }
                    else
                    {
                        Context.ActivePlayers.Add(player);
                    }
                }
            }

            if (model.DisconnectedPlayers != null)
            {
                foreach (PlayerDisconnectedModel player in model.DisconnectedPlayers)
                {
                    PlayerDisconnectedModel existingPlayer = await Context.DisconnectedPlayers
                        .FirstOrDefaultAsync(p => p.Id == player.Id);

                    if (existingPlayer != null)
                    {
                        Context.Entry(existingPlayer).CurrentValues.SetValues(player);
                    }
                    else
                    {
                        Context.DisconnectedPlayers.Add(player);
                    }
                }
            }

            await Context.SaveChangesAsync();
        }
    }
}