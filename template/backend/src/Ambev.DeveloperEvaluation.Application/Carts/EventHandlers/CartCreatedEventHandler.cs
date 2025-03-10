using Ambev.DeveloperEvaluation.Common.Messaging;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Carts.EventHandlers;

/// <summary>
/// Handles the <see cref="CartCreatedEvent"/> by logging the event and publishing to a message broker.
/// </summary>
public class CartCreatedEventHandler : INotificationHandler<CartCreatedEvent>
{
    private readonly ILogger<CartCreatedEventHandler> _logger;
    private readonly IMessageBroker? _messageBroker;

    /// <summary>
    /// Initializes a new instance of the <see cref="CartCreatedEventHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="messageBroker">The message broker service (optional).</param>
    public CartCreatedEventHandler(ILogger<CartCreatedEventHandler> logger, IMessageBroker? messageBroker = null)
    {
        _logger = logger;
        _messageBroker = messageBroker;
    }

    /// <summary>
    /// Handles the <see cref="CartCreatedEvent"/> by logging the event details and publishing to a message broker if available.
    /// </summary>
    /// <param name="notification">The event notification.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(CartCreatedEvent notification, CancellationToken cancellationToken)
    {
        var cart = notification.Cart;
        
        _logger.LogInformation(
            "Cart created event: CartId={CartId}, CustomerName={CustomerName}, BranchName={BranchName}, TotalAmount={TotalAmount}, ProductCount={ProductCount}",
            cart.Id,
            cart.CustomerName,
            cart.BranchName,
            cart.TotalAmount,
            cart.Products.Count);

        // Publish to message broker if available
        if (_messageBroker != null)
        {
            await _messageBroker.PublishAsync("cart.created", notification, cancellationToken);
        }
    }
} 