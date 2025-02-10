using AutoMapper;
using ZeissAssessment.API.Models;
using ZeissAssessment.API.Models.DTOs;
using ZeissAssessment.API.Repositories.Interfaces;
using ZeissAssessment.API.Services.Interfaces;

namespace ZeissAssessment.API.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductDto> AddProduct(AddProductRequestDto product)
    {
        var addedProduct = await _productRepository.AddProduct(_mapper.Map<Product>(product));
        return _mapper.Map<ProductDto>(addedProduct);
    }

    public async Task<ProductDto?> AddStock(int id, int quantity)
    {
        var product = await _productRepository.GetProduct(id);
        if (product == null)
        {
            return null;
        }
        product.Stock += quantity;

        var updatedProduct = await _productRepository.UpdateProduct(product);
        return _mapper.Map<ProductDto>(updatedProduct);
    }

    public async Task<ProductDto?> DecreaseStock(int id, int quantity)
    {
        var product = await _productRepository.GetProduct(id);
        if (product == null)
        {
            return null;
        }
        product.Stock -= quantity;

        var updatedProduct = await _productRepository.UpdateProduct(product);
        return _mapper.Map<ProductDto>(updatedProduct);
    }

    public Task<Product?> DeleteProduct(int id)
    {
        return _productRepository.DeleteProduct(id);
    }

    public async Task<ProductDto?> GetProduct(int id)
    {
        var product = await _productRepository.GetProduct(id);

        if (product == null)
        {
            return null;
        }
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        var products = await _productRepository.GetProducts();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetProducts(string filteredbyName)
    {
        var products = await _productRepository.GetProducts(filteredbyName);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetProducts(int minStock, int maxStock)
    {
        var products = await _productRepository.GetProducts(minStock: minStock, maxStock: maxStock);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> UpdateProduct(ProductDto product)
    {
        var updatedProduct = await _productRepository.UpdateProduct(_mapper.Map<Product>(product));
        if (updatedProduct == null)
        {
            return null;
        }
        return _mapper.Map<ProductDto>(updatedProduct);
    }
}