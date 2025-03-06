using MediatR;
using Ambev.DeveloperEvaluation.Application.Common;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Command for updating an existing product.
/// Implements IUserCommand to support automatic user information population.
/// </summary>
public class UpdateProductCommand : IRequest<UpdateProductResult>, IUserCommand
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

    /// <summary>
    /// Gets or sets the status of the product (Active or Discontinued).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the external identifier of the user associated with the command.
    /// </summary>
    public string CustomerExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name of the user associated with the command.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;
} 