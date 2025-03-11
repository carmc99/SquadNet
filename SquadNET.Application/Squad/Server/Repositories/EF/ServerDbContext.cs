// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Server.Repositories.EF
{
    public class ServerDbContext : DbContext
    {
        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
        {
        }

        public DbSet<ServerInformationModel> ServerInformationModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServerInformationModel>()
                .HasKey(s => s.ServerName);
        }
    }
}