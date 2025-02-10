using System.ComponentModel.DataAnnotations;

namespace ZeissAssessment.API.Models.DTOs;

/// <summary>
/// Data Transfer Object for Product
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Gets or sets the product ID.
    /// </summary>
    [Required]
    [Range(100000, 999999)]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    [Required]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the stock quantity.
    /// </summary>
    [Required]
    [Range(Int32.MinValue, Int32.MaxValue)]
    public int? Stock { get; set; }
}