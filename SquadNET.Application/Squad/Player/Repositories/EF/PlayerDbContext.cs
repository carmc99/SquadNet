// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Server.Repositories.EF
{
    public class PlayerDbContext : DbContextBase
    {
        public PlayerDbContext(IConfiguration configuration) : base(configuration)
        {
        }

        public DbSet<PlayerConnectedModel> ActivePlayers { get; set; }
        public DbSet<PlayerDisconnectedModel> DisconnectedPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreatorOnlineModel>()
                 .HasKey(c => c.Id);

            modelBuilder.Entity<PlayerConnectedModel>()
                .HasKey(p => p.CreatorId);

            modelBuilder.Entity<PlayerConnectedModel>()
                .HasOne(p => p.CreatorIds)
                .WithOne()
                .HasForeignKey<PlayerConnectedModel>(p => p.CreatorId);

            modelBuilder.Entity<PlayerDisconnectedModel>()
                .HasKey(t => t.Id);
        }
    }
}