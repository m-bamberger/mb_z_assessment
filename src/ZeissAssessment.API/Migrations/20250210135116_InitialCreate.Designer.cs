﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZeissAssessment.API.Data;

#nullable disable

namespace ZeissAssessment.API.Migrations
{
    [DbContext(typeof(ProductsDbContext))]
    [Migration("20250210135116_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence<int>("ProductIdSequence", "dbo")
                .StartsAt(100005L)
                .HasMin(100000L)
                .HasMax(999999L);

            modelBuilder.Entity("ZeissAssessment.API.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR dbo.ProductIdSequence");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 100000,
                            Name = "Product 1",
                            Stock = 10
                        },
                        new
                        {
                            Id = 100001,
                            Name = "Product 2",
                            Stock = 20
                        },
                        new
                        {
                            Id = 100002,
                            Name = "Product 3",
                            Stock = 30
                        },
                        new
                        {
                            Id = 100003,
                            Name = "Product 4",
                            Stock = 40
                        },
                        new
                        {
                            Id = 100004,
                            Name = "Product 5",
                            Stock = 50
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
