using ZeissAssessment.API.Models;
using ZeissAssessment.API.Models.DTOs;

namespace ZeissAssessment.API.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProducts();
    Task<ProductDto> AddProduct(AddProductRequestDto product);
    Task<ProductDto?> GetProduct(int id);
    Task<ProductDto?> UpdateProduct(ProductDto product);
    Task<Product?> DeleteProduct(int id);

    Task<ProductDto?> DecreaseStock(int id, int quantity);
    Task<ProductDto?> AddStock(int id, int quantity);
    Task<IEnumerable<ProductDto>> GetProducts(string filteredbyName);
    Task<IEnumerable<ProductDto>> GetProducts(int minStock, int maxStock);
}