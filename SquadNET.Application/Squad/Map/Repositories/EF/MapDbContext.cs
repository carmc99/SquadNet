// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Server.Repositories.EF
{
    public class MapDbContext : DbContext
    {
        public MapDbContext(DbContextOptions<MapDbContext> options) : base(options)
        {
        }

        public DbSet<CurrentMapModel> CurrentMap { get; set; }
        public DbSet<LayerModel> Layers { get; set; }
        public DbSet<NextMapModel> NextMap { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrentMapModel>()
                .HasKey(c => c.Name);

            modelBuilder.Entity<NextMapModel>()
                .HasKey(c => c.Name);

            modelBuilder.Entity<LayerModel>()
                .HasKey(l => l.Name);
        }
    }
}