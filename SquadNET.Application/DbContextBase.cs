// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SquadNET.Application.Squad.Server.Repositories.EF
{
    public abstract class DbContextBase : DbContext
    {
        private readonly IConfiguration Configuration;

        protected DbContextBase(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public static void ConfigureDatabaseProvider(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            string dbProvider = configuration.GetSection("Database:Provider").Value;
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(dbProvider))
            {
                throw new InvalidOperationException("Database provider is not specified in the configuration.");
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is not specified in the configuration.");
            }

            switch (dbProvider.ToLowerInvariant())
            {
                case "sqlserver":
                    optionsBuilder.UseSqlServer(connectionString);
                    break;

                case "sqlite":
                    optionsBuilder.UseSqlite(connectionString);
                    break;

                case "inmemory":
                    optionsBuilder.UseInMemoryDatabase(connectionString);
                    break;

                default:
                    throw new NotSupportedException($"Database provider '{dbProvider}' is not supported.");
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                ConfigureDatabaseProvider(optionsBuilder, Configuration);
            }
        }
    }
}