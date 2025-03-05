using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

public class SaleCancelledEvent : INotification
{
    public Sale Sale { get; private set; }

    public SaleCancelledEvent(Sale sale)
    {
        Sale = sale;
    }
}
