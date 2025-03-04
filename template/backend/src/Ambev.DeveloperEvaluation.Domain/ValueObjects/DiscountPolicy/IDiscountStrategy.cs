namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy;

/// <summary>
/// Defines a strategy for calculating a discount based on the quantity of items.
/// </summary>
public interface IDiscountStrategy
{
    /// <summary>
    /// Calculates the discount percentage for the given quantity.
    /// </summary>
    /// <param name="quantity">The quantity of items.</param>
    /// <returns>The discount percentage as a decimal.</returns>
    decimal CalculateDiscount(int quantity);
}
