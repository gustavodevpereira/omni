using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Query to retrieve a product by its unique identifier.
/// </summary>
public class GetProductQuery : IRequest<GetProductResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to retrieve.
    /// </summary>
    public Guid Id { get; set; }
} 