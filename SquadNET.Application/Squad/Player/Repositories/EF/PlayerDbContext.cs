// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using SquadNET.Core.Squad.Models;

namespace SquadNET.Application.Squad.Server.Repositories.EF
{
    public class PlayerDbContext : DbContext
    {
        public PlayerDbContext(DbContextOptions<PlayerDbContext> options) : base(options)
        {
        }

        public DbSet<ListPlayerModel> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}