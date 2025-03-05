using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

public class SaleCreatedEvent : INotification
{
    public Sale Sale { get; private set; }

    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
    }
}
