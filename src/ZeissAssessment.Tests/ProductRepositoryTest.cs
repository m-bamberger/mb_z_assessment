using Microsoft.EntityFrameworkCore;
using ZeissAssessment.API.Data;
using ZeissAssessment.API.Models;
using ZeissAssessment.API.Repositories;

namespace ZeissAssessment.Tests;

public class ProductRepositoryTests : IDisposable
{
    private readonly ProductsDbContext _dbContext;
    private readonly ProductRepository _productRepository;

    public ProductRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _dbContext = new ProductsDbContext(options);
        _productRepository = new ProductRepository(_dbContext);
    }

    [Fact]
    public async Task GetProducts_ReturnsAllProducts()
    {
        // Arrange        
        _dbContext.Products.AddRange(new List<Product>
        {
            new Product("Product 01", 10),
            new Product("Product 02", 20)
        });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetProducts_WithFilteredByName_ReturnsFilteredProducts()
    {
        // Arrange        
        _dbContext.Products.AddRange(new List<Product>
        {
            new Product("Product 01", 10),
            new Product("Test Product 02", 20),
            new Product("Test Product 03", 30)
        });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetProducts("Test");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, p => Assert.Contains("Test", p.Name));
    }

    [Fact]
    public async Task GetProducts_WithMinStockAndMaxStock_ReturnsFilteredProducts()
    {
        // Arrange        
        _dbContext.Products.AddRange(new List<Product>
        {
            new Product("Product 01", 10),
            new Product("Product 02", 20),
            new Product("Product 03", 30),
            new Product("Product 04", 40)
        });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetProducts(minStock: 15, maxStock: 35);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, p => Assert.InRange(p.Stock, 15, 35));
    }

    [Fact]
    public async Task GetProduct_ReturnsProduct()
    {
        // Arrange
        var product = new Product("Product 03", 30) { Id = 100001 };
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetProduct(100001);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(product.Stock, result.Stock);
    }

    [Fact]
    public async Task AddProduct_AddsProductToDatabase()
    {
        // Arrange
        var product = new Product("Product 1", 10);

        // Act
        var result = await _productRepository.AddProduct(product);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(1, _dbContext.Products.Count());
    }

    [Fact]
    public async Task UpdateProduct_UpdatesExistingProduct()
    {
        // Arrange
        var product = new Product("Product 1", 10) { Id = 1 };
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var updatedProduct = new Product("Updated Product", 20) { Id = 1 };

        // Act
        var result = await _productRepository.UpdateProduct(updatedProduct);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedProduct.Name, result.Name);
        Assert.Equal(updatedProduct.Stock, result.Stock);
    }

    [Fact]
    public async Task DeleteProduct_RemovesProductFromDatabase()
    {
        // Arrange
        var product = new Product("Product 1", 10) { Id = 1 };
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.DeleteProduct(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Empty(_dbContext.Products);
    }

    [Fact]
    public async Task GetProducts_WithNoProducts_ReturnsEmptyList()
    {
        // Act
        var result = await _productRepository.GetProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetProduct_WithNonExistentId_ReturnsNull()
    {
        // Act
        var result = await _productRepository.GetProduct(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateProduct_WithNonExistentProduct_ReturnsNull()
    {
        // Arrange
        var updatedProduct = new Product("Updated Product", 20) { Id = 999 };

        // Act
        var result = await _productRepository.UpdateProduct(updatedProduct);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteProduct_WithNonExistentId_ReturnsNull()
    {
        // Act
        var result = await _productRepository.DeleteProduct(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddProduct_WithNullProduct_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _productRepository.AddProduct(null));
    }      

    [Fact]
    public async Task DeleteProduct_WithNegativeId_ReturnsNull()
    {
        // Act
        var result = await _productRepository.DeleteProduct(-1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProducts_WithExactStock_ReturnsFilteredProducts()
    {
        // Arrange        
        _dbContext.Products.AddRange(new List<Product>
        {
            new Product("Product 01", 10),
            new Product("Product 02", 20),
            new Product("Product 03", 30)
        });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetProducts(minStock: 20, maxStock: 20);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(20, result.First().Stock);
    }

    [Fact]
    public async Task GetProducts_WithStockOutOfRange_ReturnsEmptyList()
    {
        // Arrange        
        _dbContext.Products.AddRange(new List<Product>
        {
            new Product("Product 01", 10),
            new Product("Product 02", 20),
            new Product("Product 03", 30)
        });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _productRepository.GetProducts(minStock: 40, maxStock: 50);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddProduct_WithDuplicateId_ThrowsException()
    {
        // Arrange
        var product1 = new Product("Product 1", 10) { Id = 1 };
        var product2 = new Product("Product 2", 20) { Id = 1 };
        _dbContext.Products.Add(product1);
        await _dbContext.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _productRepository.AddProduct(product2));
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}