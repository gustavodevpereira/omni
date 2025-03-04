using Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy.Strategies;

namespace Ambev.DeveloperEvaluation.Domain.ValueObjects.DiscountPolicy;

public record DiscountPolicy
{
    private readonly List<(Func<int, bool> Condition, IDiscountStrategy Strategy)> _strategies;

    public DiscountPolicy()
    {
        _strategies =
        [
            (q => q < 4, new NoDiscountStrategy()),
            (q => q >= 4 && q < 10, new TenPercentDiscountStrategy()),
            (q => q >= 10 && q <= 20, new TwentyPercentDiscountStrategy())
        ];
    }

    public decimal GetDiscountPercentage(int quantity)
    {
        var selectedStrategy = _strategies.FirstOrDefault(s => s.Condition(quantity));

        if (selectedStrategy.Equals(default))
            throw new DomainException("The quantity cannot be higher than 20");

        return selectedStrategy.Strategy.CalculateDiscount(quantity);
    }

}
