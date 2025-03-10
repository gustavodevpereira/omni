using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

/// <summary>
/// Specification that determines if a cart is active.
/// Used to filter carts based on their active status.
/// </summary>
public class ActiveCartSpecification : ISpecification<Cart>
{
    /// <summary>
    /// Determines whether the specified cart is active.
    /// </summary>
    /// <param name="cart">The cart to check.</param>
    /// <returns>True if the cart status is Active; otherwise, false.</returns>
    public bool IsSatisfiedBy(Cart cart)
    {
        return cart.Status == CartStatus.Active;
    }
} 