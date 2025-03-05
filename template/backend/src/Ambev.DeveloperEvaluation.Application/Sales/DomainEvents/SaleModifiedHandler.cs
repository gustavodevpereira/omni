using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DomainEvents;

public class SaleModifiedHandler : INotificationHandler<SaleModifiedEvent>
{
    private readonly ILogger<SaleModifiedHandler> _logger;

    public SaleModifiedHandler(ILogger<SaleModifiedHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"The sale with id <{notification.Sale.Id}> was modified");
    }
}
