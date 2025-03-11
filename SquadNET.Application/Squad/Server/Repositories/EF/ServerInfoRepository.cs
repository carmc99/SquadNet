// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Server.Repositories.EF
{
    internal class ServerInfoRepository : IServerInfoRepository
    {
        private readonly ServerDbContext Context;

        public ServerInfoRepository(IDbContextFactory<ServerDbContext> contextFactory)
        {
            Context = contextFactory.CreateDbContext();
        }

        public async Task Store(ServerInformationModel model)
        {
            ServerInformationModel existingEntity = await Context.ServerInformationModels
                .FirstOrDefaultAsync(s => s.ServerName == model.ServerName);

            if (existingEntity != null)
            {
                Context.Entry(existingEntity).CurrentValues.SetValues(model);
            }
            else
            {
                Context.Set<ServerInformationModel>().Add(model);
            }

            await Context.SaveChangesAsync();
        }
    }
}