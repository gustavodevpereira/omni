using Ambev.DeveloperEvaluation.Domain.Events.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales;

public class SaleCancelledHandler : INotificationHandler<SaleCancelledEvent>
{
    public Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
