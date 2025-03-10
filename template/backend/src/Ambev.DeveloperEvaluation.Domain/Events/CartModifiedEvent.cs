using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a cart/sale is modified in the system.
/// </summary>
public class CartModifiedEvent : INotification
{
    /// <summary>
    /// Gets the cart that was modified.
    /// </summary>
    public Cart Cart { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CartModifiedEvent"/> class.
    /// </summary>
    /// <param name="cart">The cart that was modified.</param>
    public CartModifiedEvent(Cart cart)
    {
        Cart = cart;
    }
} 