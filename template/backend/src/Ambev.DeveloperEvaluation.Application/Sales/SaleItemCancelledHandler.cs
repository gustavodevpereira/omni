using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public class SaleItemCancelledHandler : INotificationHandler<SaleItemCancelledEvent>
{
    public Task Handle(SaleItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
