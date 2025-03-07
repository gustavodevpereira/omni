﻿namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.UpdateProduct;

/// <summary>
/// Represents a request to update an existing product.
/// </summary>
public class UpdateProductRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SKU (Stock Keeping Unit) of the product.
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the stock quantity of the product.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets the category of the product.
    /// </summary>
    public string Category { get; set; } = string.Empty;
}
