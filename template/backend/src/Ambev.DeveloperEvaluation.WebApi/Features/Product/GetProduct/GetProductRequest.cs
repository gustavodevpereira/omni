namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProduct;

/// <summary>
/// Represents a request to get a product by its ID.
/// </summary>
public class GetProductRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to retrieve.
    /// </summary>
    public Guid Id { get; set; }
}
