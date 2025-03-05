using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Events.Sales;

/// <summary>
/// Domain event that is raised when a sale item is cancelled/removed.
/// </summary>
public sealed class ItemCancelledEvent : IDomainEvent
{
    /// <summary>
    /// Gets the sale from which the item was removed.
    /// </summary>
    public Sale Sale { get; }
    
    /// <summary>
    /// Gets the ID of the removed sale item.
    /// </summary>
    public Guid RemovedSaleItemId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemCancelledEvent"/> class.
    /// </summary>
    /// <param name="sale">The sale from which the item was removed.</param>
    /// <param name="removedSaleItemId">The ID of the removed sale item.</param>
    public ItemCancelledEvent(Sale sale, Guid removedSaleItemId)
    {
        Sale = sale;
        RemovedSaleItemId = removedSaleItemId;
    }
} 