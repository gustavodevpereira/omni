namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy.Strategies;

/// <summary>
/// Represents the discount strategy that applies a 10% discount.
/// This strategy is used for quantities between 4 and 9.
/// </summary>
public class TenPercentDiscountStrategy : IDiscountStrategy
{
    /// <summary>
    /// Calculates a 10% discount regardless of the specific quantity.
    /// </summary>
    /// <param name="quantity">The quantity of items (not used in this strategy).</param>
    /// <returns>0.1, representing a 10% discount.</returns>
    public decimal CalculateDiscount(int quantity) => 0.1m;
}
