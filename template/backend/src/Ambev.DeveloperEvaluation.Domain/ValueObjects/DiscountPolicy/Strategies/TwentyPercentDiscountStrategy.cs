namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy.Strategies;

/// <summary>
/// Represents the discount strategy that applies a 20% discount.
/// This strategy is used for quantities between 10 and 20.
/// </summary>
public class TwentyPercentDiscountStrategy : IDiscountStrategy
{
    /// <summary>
    /// Calculates a 20% discount regardless of the specific quantity.
    /// </summary>
    /// <param name="quantity">The quantity of items (not used in this strategy).</param>
    /// <returns>0.2, representing a 20% discount.</returns>
    public decimal CalculateDiscount(int quantity) => 0.2m;
}
