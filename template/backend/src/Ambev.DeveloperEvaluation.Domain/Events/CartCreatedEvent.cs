using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a new cart/sale is created in the system.
/// </summary>
public class CartCreatedEvent : INotification
{
    /// <summary>
    /// Gets the cart that was created.
    /// </summary>
    public Cart Cart { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CartCreatedEvent"/> class.
    /// </summary>
    /// <param name="cart">The cart that was created.</param>
    public CartCreatedEvent(Cart cart)
    {
        Cart = cart;
    }
} 