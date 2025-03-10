using Ambev.DeveloperEvaluation.Common.Messaging;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.EventHandlers;

/// <summary>
/// Handles the <see cref="CartCancelledEvent"/> by logging the event and publishing to a message broker.
/// </summary>
public class CartCancelledEventHandler : INotificationHandler<CartCancelledEvent>
{
    private readonly ILogger<CartCancelledEventHandler> _logger;
    private readonly IMessageBroker? _messageBroker;

    /// <summary>
    /// Initializes a new instance of the <see cref="CartCancelledEventHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageBroker">The message broker service (optional).</param>
    public CartCancelledEventHandler(ILogger<CartCancelledEventHandler> logger, IMessageBroker? messageBroker = null)
    {
        _logger = logger;
        _messageBroker = messageBroker;
    }

    /// <summary>
    /// Handles the <see cref="CartCancelledEvent"/> by logging the event details and publishing to a message broker if available.
    /// </summary>
    /// <param name="notification">The event notification.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(CartCancelledEvent notification, CancellationToken cancellationToken)
    {
        var cart = notification.Cart;
        
        _logger.LogInformation(
            "Cart cancelled event: CartId={CartId}, CustomerName={CustomerName}, TotalAmount={TotalAmount}",
            cart.Id,
            cart.CustomerName,
            cart.TotalAmount);

        // Publish to message broker if available
        if (_messageBroker != null)
        {
            await _messageBroker.PublishAsync("cart.cancelled", notification, cancellationToken);
        }
    }
} 