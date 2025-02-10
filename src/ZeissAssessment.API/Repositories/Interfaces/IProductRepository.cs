using ZeissAssessment.API.Models;

namespace ZeissAssessment.API.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts(string? filteredbyName = null, int? minStock = null, int? maxStock = null);
    Task<Product?> GetProduct(int id);
    Task<Product> AddProduct(Product product);
    Task<Product?> UpdateProduct(Product product);
    Task<Product?> DeleteProduct(int id);
}