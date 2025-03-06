using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Request model for creating a new sale
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// The unique sale number
    /// </summary>
    [Required]
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The date when the sale was made
    /// </summary>
    [Required]
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The external customer ID (Auto-preenchido com o usu�rio autenticado)
    /// </summary>
    [JsonIgnore]
    [SwaggerSchema(ReadOnly = true)]
    public string CustomerExternalId { get; set; } = string.Empty;

    /// <summary>
    /// The customer name (Auto-preenchido com o usu�rio autenticado)
    /// </summary>
    [JsonIgnore]
    [SwaggerSchema(ReadOnly = true)]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// The external branch ID
    /// </summary>
    [Required]
    public string BranchExternalId { get; set; } = string.Empty;

    /// <summary>
    /// The branch name
    /// </summary>
    [Required]
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// The list of sale items
    /// </summary>
    [Required]
    public List<CreateSaleItemRequest> Items { get; set; } = new List<CreateSaleItemRequest>();
}

/// <summary>
/// Request model for creating a sale item
/// </summary>
public class CreateSaleItemRequest
{
    /// <summary>
    /// The external product ID
    /// </summary>
    [Required]
    public string ProductExternalId { get; set; } = string.Empty;

    /// <summary>
    /// The product name
    /// </summary>
    [Required] 
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// The quantity of the product
    /// </summary>
    [Required]
    [Range(1, 20, ErrorMessage = "Quantity must be between 1 and 20")]
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the product
    /// </summary>
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero")]
    public decimal UnitPrice { get; set; }
} 