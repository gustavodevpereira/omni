using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public class SaleModifiedHandler : INotificationHandler<SaleModifiedEvent>
{
    public Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
