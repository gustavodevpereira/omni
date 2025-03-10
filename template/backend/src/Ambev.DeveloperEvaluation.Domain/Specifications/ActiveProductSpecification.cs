using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

/// <summary>
/// Specification that determines if a product is active.
/// Used to filter products based on their active status.
/// </summary>
public class ActiveProductSpecification : ISpecification<Product>
{
    /// <summary>
    /// Determines whether the specified product is active.
    /// </summary>
    /// <param name="product">The product to check.</param>
    /// <returns>True if the product status is Active; otherwise, false.</returns>
    public bool IsSatisfiedBy(Product product)
    {
        return product.Status == ProductStatus.Active;
    }
} 