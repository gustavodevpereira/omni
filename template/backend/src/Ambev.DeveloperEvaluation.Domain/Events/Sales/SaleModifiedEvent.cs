using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

public class SaleModifiedEvent : INotification
{
    public Sale Sale { get; private set; }

    public SaleModifiedEvent(Sale sale)
    {
        Sale = sale;
    }

}
