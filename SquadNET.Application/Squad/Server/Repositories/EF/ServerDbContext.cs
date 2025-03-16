// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Server.Repositories.EF
{
    public class ServerDbContext : DbContextBase
    {
        public ServerDbContext(IConfiguration configuration) : base(configuration)
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