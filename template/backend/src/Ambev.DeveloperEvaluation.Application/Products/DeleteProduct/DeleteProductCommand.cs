using MediatR;
using Ambev.DeveloperEvaluation.Application.Common;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct;

/// <summary>
/// Command for deleting a product.
/// Implements IUserCommand to support automatic user information population.
/// </summary>
public class DeleteProductCommand : IRequest<bool>, IUserCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to delete.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the external identifier of the user associated with the command.
    /// </summary>
    public string CustomerExternalId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name of the user associated with the command.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;
} 