using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities.Products;

/// <summary>
/// Represents a product in the system with its details and status.
/// This entity follows domain-driven design principles and includes business rules validation.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Gets or sets the product's name.
    /// Must not be null or empty.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product's description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product's SKU (Stock Keeping Unit).
    /// Must be unique and follow the correct format.
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product's price.
    /// Must be greater than zero.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the product's stock quantity.
    /// Must be non-negative.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets the product's category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product's current status.
    /// Indicates whether the product is active or discontinued.
    /// </summary>
    public ProductStatus Status { get; set; }

    /// <summary>
    /// Gets the date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets the date and time of the last update to the product's information.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the Product class.
    /// </summary>
    public Product()
    {
        CreatedAt = DateTime.UtcNow;
        Status = ProductStatus.Active;
    }

    /// <summary>
    /// Activates the product.
    /// Changes the product's status to Active.
    /// </summary>
    public void Activate()
    {
        Status = ProductStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Discontinues the product.
    /// Changes the product's status to Discontinued.
    /// </summary>
    public void Discontinue()
    {
        Status = ProductStatus.Discontinued;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the stock quantity of the product.
    /// </summary>
    /// <param name="quantity">The new stock quantity</param>
    /// <exception cref="DomainException">Thrown if the quantity is negative</exception>
    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new DomainException("Stock quantity cannot be negative.");

        StockQuantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }
} 