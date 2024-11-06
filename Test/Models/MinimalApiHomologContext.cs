using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Test.Models;

public partial class MinimalApiHomologContext : DbContext
{
    public MinimalApiHomologContext()
    {
    }

    public MinimalApiHomologContext(DbContextOptions<MinimalApiHomologContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=minimal_api_homolog;uid=root;pwd=dio123", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.5.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("administrators")
                .UseCollation("utf8mb4_uca1400_ai_ci");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Profile)
                .HasMaxLength(10)
                .HasCharSet("utf8mb4");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity
                .ToTable("__efmigrationshistory")
                .UseCollation("utf8mb4_uca1400_ai_ci");

            entity.Property(e => e.MigrationId)
                .HasMaxLength(150)
                .HasCharSet("utf8mb4");
            entity.Property(e => e.ProductVersion)
                .HasMaxLength(32)
                .HasCharSet("utf8mb4");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("vehicles")
                .UseCollation("utf8mb4_uca1400_ai_ci");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Make)
                .HasMaxLength(100)
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Model)
                .HasMaxLength(150)
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Year).HasColumnType("int(11)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
