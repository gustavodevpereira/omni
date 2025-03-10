using Ambev.DeveloperEvaluation.Application.Products.Common.Results;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Response model for a product in a paginated list
/// </summary>
public class GetProductsResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the product
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SKU of the product
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price of the product
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the stock quantity of the product
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets the category of the product
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch identifier of the product
    /// </summary>
    public Guid BranchExternalId { get; set; }

    /// <summary>
    /// Gets or sets the branch name of the product
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the product
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date of the product
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last update date of the product
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}