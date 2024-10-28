﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MinimalApi.Infrastructure.Db;

#nullable disable

namespace MinimalApi.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20241028035927_VehicleMigrationAditionalVehicles")]
    partial class VehicleMigrationAditionalVehicles
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("MinimalApi.Domain.Entities.Administrator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Profie")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Administrators");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@test.com",
                            Password = "123456",
                            Profie = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Email = "john@doe.com",
                            Password = "123",
                            Profie = "User"
                        },
                        new
                        {
                            Id = 3,
                            Email = "super@doe.com",
                            Password = "123",
                            Profie = "Superuser"
                        });
                });

            modelBuilder.Entity("MinimalApi.Domain.Entities.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Vehicles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Make = "Volkswagen",
                            Model = "Polo CL",
                            Year = 1999
                        },
                        new
                        {
                            Id = 2,
                            Make = "Renault",
                            Model = "Clio RN",
                            Year = 1996
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
