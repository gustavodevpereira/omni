namespace Ambev.DeveloperEvaluation.Application.Products.Common.Results;

/// <summary>
/// Represents the response returned after successfully retrieving a product.
/// </summary>
public class ProductResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product's description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product's SKU (Stock Keeping Unit).
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product's price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the product's stock quantity.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets the product's category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the external identifier for the branch where the product belongs.
    /// </summary>
    public Guid BranchExternalId { get; set; }

    /// <summary>
    /// Gets or sets the branch name of the product.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product's current status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets the date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the product's information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 