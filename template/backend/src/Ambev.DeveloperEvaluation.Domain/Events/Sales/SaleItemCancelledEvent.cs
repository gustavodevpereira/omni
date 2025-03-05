using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

public class SaleItemCancelledEvent : INotification
{
    public SaleItem SaleItem { get; private set; }

    public SaleItemCancelledEvent(SaleItem saleItem)
    {
        SaleItem = saleItem;
    }
}
