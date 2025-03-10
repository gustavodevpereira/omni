using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a cart/sale is cancelled in the system.
/// </summary>
public class CartCancelledEvent : INotification
{
    /// <summary>
    /// Gets the cart that was cancelled.
    /// </summary>
    public Cart Cart { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CartCancelledEvent"/> class.
    /// </summary>
    /// <param name="cart">The cart that was cancelled.</param>
    public CartCancelledEvent(Cart cart)
    {
        Cart = cart;
    }
} 