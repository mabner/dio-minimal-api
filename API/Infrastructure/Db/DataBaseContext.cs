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
                        Profile = "Admin",
                    },
                    new Administrator
                    {
                        Id = 2,
                        Email = "john@doe.com",
                        Password = "123",
                        Profile = "User",
                    },
                    new Administrator
                    {
                        Id = 3,
                        Email = "super@doe.com",
                        Password = "123",
                        Profile = "Superuser",
                    }
                );

            modelBuilder
                .Entity<Vehicle>()
                .HasData(
                    new Vehicle
                    {
                        Id = 1,
                        Model = "Polo CL",
                        Make = "Volkswagen",
                        Year = 1999,
                    },
                    new Vehicle
                    {
                        Id = 2,
                        Model = "Clio RN",
                        Make = "Renault",
                        Year = 1996,
                    }
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Passing the config in the contructor
            if (!optionsBuilder.IsConfigured)
            {
                var connString = _appSettingsConfig.GetConnectionString("MySql")?.ToString();
                if (!string.IsNullOrEmpty(connString))
                {
                    optionsBuilder.UseMySql(connString, ServerVersion.AutoDetect(connString));
                }
            }
        }
    }
}
