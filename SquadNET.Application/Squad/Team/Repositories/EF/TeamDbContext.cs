// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Team.Repositories.EF
{
    public class TeamDbContext : DbContext
    {
        public TeamDbContext(DbContextOptions<TeamDbContext> options) : base(options)
        {
        }

        public DbSet<TeamModel> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamModel>()
                .HasKey(t => t.Id);
        }
    }
}