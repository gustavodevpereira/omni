using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public class SaleItemCancelledHandler : INotificationHandler<SaleItemCancelledEvent>
{
    private readonly ILogger<SaleItemCancelledHandler> _logger;

    public SaleItemCancelledHandler(ILogger<SaleItemCancelledHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"The sale item with Id <{notification.SaleItem.Id}> was cancelled");
    }
}
