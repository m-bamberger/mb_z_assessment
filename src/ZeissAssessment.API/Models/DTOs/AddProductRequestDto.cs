using System.ComponentModel.DataAnnotations;

namespace ZeissAssessment.API.Models.DTOs;

/// <summary>
/// DTO for adding a product request.
/// </summary>
public class AddProductRequestDto
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    [Required]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the stock of the product.
    /// </summary>
    [Required]
    [Range(Int32.MinValue, Int32.MaxValue)]
    public int? Stock { get; set; }
}