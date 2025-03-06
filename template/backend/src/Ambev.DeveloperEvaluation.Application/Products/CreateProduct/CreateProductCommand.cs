using MediatR;
using Ambev.DeveloperEvaluation.Application.Common;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Command for creating a new product.
/// Implements IUserCommand to support automatic user information population.
/// </summary>
public class CreateProductCommand : IRequest<CreateProductResult>, IUserCommand
{
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
    /// Gets or sets the initial stock quantity of the product.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets the category of the product.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the external identifier of the user associated with the command.
    /// </summary>
    public string CustomerExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name of the user associated with the command.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;
} 