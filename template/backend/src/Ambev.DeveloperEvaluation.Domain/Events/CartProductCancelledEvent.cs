using MediatR;
using Ambev.DeveloperEvaluation.Domain.Entities.Carts;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered when a product/item is removed from a cart in the system.
/// </summary>
public class CartProductCancelledEvent : INotification
{
    /// <summary>
    /// Gets the cart from which the product was removed.
    /// </summary>
    public Cart Cart { get; }

    /// <summary>
    /// Gets the ID of the cart product that was removed.
    /// </summary>
    public Guid CartProductId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CartProductCancelledEvent"/> class.
    /// </summary>
    /// <param name="cart">The cart from which the product was removed.</param>
    /// <param name="cartProductId">The ID of the cart product that was removed.</param>
    public CartProductCancelledEvent(Cart cart, Guid cartProductId)
    {
        Cart = cart;
        CartProductId = cartProductId;
    }
} 