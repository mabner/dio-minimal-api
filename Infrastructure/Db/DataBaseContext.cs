using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Infrastructure.Db
{
    public class DataBaseContext : DbContext
    {
        // Dependency injection ---->
        private readonly IConfiguration _appSettingsConfig;

        public DataBaseContext(IConfiguration appSettingsConfig)
        {
            _appSettingsConfig = appSettingsConfig;
        }

        // <-----
        public DbSet<Administrator> Administrators { get; set; } = default!;
        public DbSet<Vehicle> Vehicles { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Administrator>()
                .HasData(
                    new Administrator
                    {
                        Id = 1,
                        Email = "admin@test.com",
                        Password = "123456",
                        Profie = "Adm",
                    }
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Passing the config in the contructor
            if (!optionsBuilder.IsConfigured)
            {
                var connString = _appSettingsConfig.GetConnectionString("mysql")?.ToString();
                if (!string.IsNullOrEmpty(connString))
                {
                    optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));
                }
            }
        }
    }
}
