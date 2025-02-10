using Microsoft.EntityFrameworkCore;
using ZeissAssessment.API.Models;

namespace ZeissAssessment.API.Data;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasSequence<int>("ProductIdSequence", schema: "dbo")
            .StartsAt(100005) // Should start at 100000, but we have 5 products in the seed data
            .HasMax(999999)
            .HasMin(100000)
            .IncrementsBy(1);

        modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

        modelBuilder.Entity<Product>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("NEXT VALUE FOR dbo.ProductIdSequence");

        var products = new[]
        {
            new Product("Product 1", 10){ Id = 100000 },
            new Product("Product 2", 20){ Id = 100001 },
            new Product("Product 3", 30){ Id = 100002 },
            new Product("Product 4", 40){ Id = 100003 },
            new Product("Product 5", 50){ Id = 100004 }
        };

        modelBuilder.Entity<Product>().HasData(products);
    }
}