using Ambev.DeveloperEvaluation.Domain.Entities.Products;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

/// <summary>
/// Specification that determines if a product is in stock.
/// Used to filter products based on their stock availability.
/// </summary>
public class InStockProductSpecification : ISpecification<Product>
{
    /// <summary>
    /// Determines whether the specified product is in stock.
    /// </summary>
    /// <param name="product">The product to check.</param>
    /// <returns>True if the product has stock quantity greater than zero; otherwise, false.</returns>
    public bool IsSatisfiedBy(Product product)
    {
        return product.StockQuantity > 0;
    }
} 