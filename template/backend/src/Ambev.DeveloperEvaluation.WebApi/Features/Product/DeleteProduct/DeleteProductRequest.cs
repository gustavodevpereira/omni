namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.DeleteProduct;

/// <summary>
/// Represents a request to delete a product.
/// </summary>
public class DeleteProductRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to delete.
    /// </summary>
    public Guid Id { get; set; }
}
