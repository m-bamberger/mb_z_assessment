using Microsoft.EntityFrameworkCore;
using ZeissAssessment.API.Data;
using ZeissAssessment.API.Models;
using ZeissAssessment.API.Repositories.Interfaces;

namespace ZeissAssessment.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductsDbContext _dbContext;

    public ProductRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Product>> GetProducts(string? filteredbyName = null, int? minStock = null, int? maxStock = null)
    {
       var products = _dbContext.Products.AsQueryable();

        if (filteredbyName is not null)
        {
            products = products.Where(x => x.Name.Contains(filteredbyName));
        }

        if (minStock is not null || maxStock is not null)
        {
            products = products.Where(x => x.Stock >= minStock && x.Stock <= maxStock);
        }

        return await products.ToListAsync();
    }

    public async Task<Product?> GetProduct(int id)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Product> AddProduct(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        Product? existingProduct = _dbContext.Products.FirstOrDefault(x => x.Id == product.Id);
        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.Name = product.Name;
        existingProduct.Stock = product.Stock;

        await _dbContext.SaveChangesAsync();

        return existingProduct;
    }

    public async Task<Product?> DeleteProduct(int id)
    {
        Product? product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
        {
            return null;
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }
}