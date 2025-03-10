using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy;

/// <summary>
/// Represents the discount policy as a value object that encapsulates the logic for calculating discounts based on quantity.
/// This policy applies different strategies based on quantity ranges to ensure business rules are followed.
/// </summary>
public record DiscountPolicy
{
    private readonly List<(Func<int, bool> Condition, IDiscountStrategy Strategy)> _strategies;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscountPolicy"/> record.
    /// Configures the discount strategies for different quantity ranges.
    /// </summary>
    public DiscountPolicy()
    {
        _strategies = new List<(Func<int, bool>, IDiscountStrategy)>
        {
            (q => q < 4, new NoDiscountStrategy()),
            (q => q >= 4 && q < 10, new TenPercentDiscountStrategy()),
            (q => q >= 10 && q <= 20, new TwentyPercentDiscountStrategy())
        };
    }

    /// <summary>
    /// Calculates the discount percentage for a given quantity using the appropriate strategy.
    /// </summary>
    /// <param name="quantity">The quantity of items.</param>
    /// <returns>The discount percentage as a decimal.</returns>
    /// <exception cref="DomainException">Thrown when the quantity is greater than 20.</exception>
    public decimal GetDiscountPercentage(int quantity)
    {
        var selectedStrategy = _strategies.FirstOrDefault(s => s.Condition(quantity));

        if (selectedStrategy.Equals(default))
            throw new DomainException("The quantity cannot be higher than 20");

        return selectedStrategy.Strategy.CalculateDiscount(quantity);
    }
}
