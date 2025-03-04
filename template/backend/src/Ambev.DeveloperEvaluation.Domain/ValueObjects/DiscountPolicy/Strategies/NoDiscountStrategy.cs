namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy.Strategies;

/// <summary>
/// Represents the discount strategy that applies no discount.
/// This strategy is used for quantities less than 4.
/// </summary>
public class NoDiscountStrategy : IDiscountStrategy
{
    /// <summary>
    /// Calculates no discount, always returning 0.
    /// </summary>
    /// <param name="quantity">The quantity of items (not used in this strategy).</param>
    /// <returns>0, indicating no discount.</returns>
    public decimal CalculateDiscount(int quantity) => 0m;
}
