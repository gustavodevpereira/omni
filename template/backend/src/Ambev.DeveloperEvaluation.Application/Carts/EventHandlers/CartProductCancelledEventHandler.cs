using Ambev.DeveloperEvaluation.Common.Messaging;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.EventHandlers;

/// <summary>
/// Handles the <see cref="CartProductCancelledEvent"/> by logging the event and publishing to a message broker.
/// </summary>
public class CartProductCancelledEventHandler : INotificationHandler<CartProductCancelledEvent>
{
    private readonly ILogger<CartProductCancelledEventHandler> _logger;
    private readonly IMessageBroker? _messageBroker;

    /// <summary>
    /// Initializes a new instance of the <see cref="CartProductCancelledEventHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageBroker">The message broker service (optional).</param>
    public CartProductCancelledEventHandler(ILogger<CartProductCancelledEventHandler> logger, IMessageBroker? messageBroker = null)
    {
        _logger = logger;
        _messageBroker = messageBroker;
    }

    /// <summary>
    /// Handles the <see cref="CartProductCancelledEvent"/> by logging the event details and publishing to a message broker if available.
    /// </summary>
    /// <param name="notification">The event notification.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(CartProductCancelledEvent notification, CancellationToken cancellationToken)
    {
        var cart = notification.Cart;
        
        _logger.LogInformation(
            "Cart product cancelled event: CartId={CartId}, CartProductId={CartProductId}, CustomerName={CustomerName}",
            cart.Id,
            notification.CartProductId,
            cart.CustomerName);

        // Publish to message broker if available
        if (_messageBroker != null)
        {
            await _messageBroker.PublishAsync("cart.product.cancelled", notification, cancellationToken);
        }
    }
} 