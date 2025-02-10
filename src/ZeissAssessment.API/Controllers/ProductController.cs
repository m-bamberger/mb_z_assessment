using Microsoft.AspNetCore.Mvc;
using ZeissAssessment.API.CustomActionFilters;
using ZeissAssessment.API.Models.DTOs;
using ZeissAssessment.API.Services.Interfaces;

namespace ZeissAssessment.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    // GET: /api/products
    /// <summary>
    /// Get all products.
    /// </summary>
    /// <returns>A list of all Products.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts()
    {
        IEnumerable<ProductDto> products = await _productService.GetProducts();
        _logger.LogInformation("Retrieved {amount} products.", products.Count());
        return Ok(products);
    }

    // GET: /api/products/{id}
    /// <summary>
    /// Get a single product by id.
    /// </summary>
    /// <param name="id">The Id of the desired product.</param>
    /// <returns>The product with the specified id.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct([FromRoute] int id)
    {
        var product = await _productService.GetProduct(id);
        if (product == null)
        {
            _logger.LogWarning("Product with the id {Id} was not found.", id);
            return NotFound();
        }
        _logger.LogInformation("Product with the id {Id} was successfully found.", id);
        return Ok(product);
    }

    // POST: /api/products
    /// <summary>
    /// Add a new product.
    /// </summary>
    /// <param name="product">The new product data.</param>
    /// <returns>The added product with the link to the new entity.</returns>
    [HttpPost]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PostProduct(AddProductRequestDto product)
    {
        var addedProduct = await _productService.AddProduct(product);
        _logger.LogInformation("Product with the id {Id} was successfully added.", addedProduct.Id);
        return CreatedAtAction("GetProduct", new { id = addedProduct.Id }, addedProduct);
    }

    // PUT: /api/product/{id}
    /// <summary>
    /// Update an existing product.
    /// </summary>
    /// <param name="id">The Id of the product to update.</param>
    /// <param name="product">The updated product data.</param>
    /// <returns>The updated product.</returns>
    [HttpPut("{id}")]
    [ValidateModel]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] ProductDto product)
    {
        if (id != product.Id)
        {
            _logger.LogWarning("Product Id {Id} does not match the product Id {product.Id}.", id, product.Id);
            return BadRequest();
        }

        ProductDto? updatedProduct = await _productService.UpdateProduct(product);
        if (updatedProduct is null)
        {
            _logger.LogWarning("Product with the id {Id} was not found.", product.Id);
            return NotFound();
        }
        _logger.LogInformation("Product with the id {Id} was successfully updatetd", product.Id);
        return Ok(updatedProduct);
    }

    // DELETE: /api/products/{id}
    /// <summary>
    /// Delete a product by id.
    /// </summary>
    /// <param name="id">The Id of the product to delete.</param>
    /// <returns>No content if the product was successfully deleted.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {
        var deletedProduct = await _productService.DeleteProduct(id);

        if (deletedProduct is null)
        {
            _logger.LogWarning("Product with the Id {Id} was not found.", id);
            return NotFound();
        }
        _logger.LogInformation("Product with the Id {Id} was successfully deleted.", deletedProduct.Id);
        return NoContent();
    }

    // PUT: /api/products/{id}/decrement-stock/{quantity}
    /// <summary>
    /// Decrease the stock of a product.
    /// </summary>
    /// <param name="id">The Id of the product.</param>
    /// <param name="quantity">The quantity to decrement.</param>
    /// <returns>The updated product.</returns>
    [HttpPut("{id}/decrement-stock/{quantity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DecrementStock([FromRoute] int id, [FromRoute] int quantity)
    {
        var product = await _productService.DecreaseStock(id, quantity);

        if (product == null)
        {
            _logger.LogWarning("Product with the Id {Id} was not found.", id);
            return NotFound();
        }
        _logger.LogInformation("Stock decreased for product with id {id} by {quanitity} to {stock}.", product.Id, quantity, product.Stock);
        return Ok(product);
    }

    // PUT: /api/products/{id}/add-to-stock/{quantity}
    /// <summary>
    /// Increase the stock of a product.
    /// </summary>
    /// <param name="id">The Id of the product.</param>
    /// <param name="quantity">The quantity to add.</param>
    /// <returns>The updated product.</returns>
    [HttpPut("{id}/add-to-stock/{quantity}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddToStock([FromRoute] int id, [FromRoute] int quantity)
    {
        var product = await _productService.AddStock(id, quantity);

        if (product == null)
        {
            _logger.LogWarning("Product with the Id {Id} was not found.", id);
            return NotFound();
        }
        _logger.LogInformation("Stock incresed for product with id {id} by {quanitity} to {stock}.", product.Id, quantity, product.Stock);
        return Ok(product);
    }

    // GET: /api/products/search?name={name}
    /// <summary>
    /// Search for products by name.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <returns>A list of products that match the search criteria.</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SearchProduct([FromQuery] string name)
    {
        var products = await _productService.GetProducts(name);
        if (products.Any() is false)
        {
            _logger.LogInformation("No products found with the name '{name}'.", name);
            return NoContent();
        }
        _logger.LogInformation("Searched for '{name}'. {amount} products found.", name, products.Count());
        return Ok(products);
    }

    // GET: /api/products/stock-level?min={min}&max={max}
    /// <summary>
    /// Get products within a specified stock level range.
    /// </summary>
    /// <param name="min">The minimum stock level.</param>
    /// <param name="max">The maximum stock level.</param>
    /// <returns>A list of products within the specified stock level range.</returns>
    [HttpGet("stock-level")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetProductsWithStockLevel([FromQuery] int? min, [FromQuery] int? max)
    {
        if (min is null || max is null)
        {
            _logger.LogWarning("Min and Max stock levels must be provided.");
            return BadRequest();
        }
        var products = await _productService.GetProducts(min.Value, max.Value);
        if (products.Any() is false)
        {
            _logger.LogInformation("No products found within the stock level range {min} - {max}.", min, max);
            return NoContent();
        }
        _logger.LogInformation("Found {amount} products within the stock level range {min} - {max}.", products.Count(), min, max);
        return Ok(products);
    }
}