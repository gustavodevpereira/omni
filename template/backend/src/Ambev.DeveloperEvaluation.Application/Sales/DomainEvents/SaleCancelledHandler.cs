using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.DomainEvents;

public class SaleCancelledHandler : INotificationHandler<SaleCancelledEvent>
{
    private readonly ILogger<SaleCancelledHandler> _logger;

    public SaleCancelledHandler(ILogger<SaleCancelledHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($" The sale with Id <{notification.Sale.Id}> was cancelled");
    }
}
