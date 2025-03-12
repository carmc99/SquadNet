// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using SquadNET.Application.Squad.Server.Repositories.EF;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Map.Repositories.EF
{
    internal class MapRepository : IMapRepository
    {
        private readonly MapDbContext Context;

        public MapRepository(IDbContextFactory<MapDbContext> contextFactory)
        {
            Context = contextFactory.CreateDbContext();
        }

        public async Task Store(List<LayerModel> model)
        {
            foreach (LayerModel layer in model)
            {
                LayerModel existingEntity = await Context.Set<LayerModel>()
                    .FirstOrDefaultAsync(l => l.Name == layer.Name);

                if (existingEntity != null)
                {
                    Context.Entry(existingEntity).CurrentValues.SetValues(layer);
                }
                else
                {
                    Context.Set<LayerModel>().Add(layer);
                }
            }

            await Context.SaveChangesAsync();
        }
    }
}