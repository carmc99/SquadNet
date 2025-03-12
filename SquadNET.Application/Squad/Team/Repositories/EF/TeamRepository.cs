// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Team.Repositories.EF
{
    internal class TeamRepository : ITeamRepository
    {
        private readonly TeamDbContext Context;

        public TeamRepository(IDbContextFactory<TeamDbContext> contextFactory)
        {
            Context = contextFactory.CreateDbContext();
        }

        public async Task Store(List<TeamModel> model)
        {
            foreach (TeamModel team in model)
            {
                TeamModel existingEntity = await Context.Teams
                .FirstOrDefaultAsync(s => s.Id == team.Id);

                if (existingEntity != null)
                {
                    Context.Entry(existingEntity).CurrentValues.SetValues(model);
                }
                else
                {
                    Context.Set<TeamModel>().Add(team);
                }
            }

            await Context.SaveChangesAsync();
        }
    }
}