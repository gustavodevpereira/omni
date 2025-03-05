using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;

/// <summary>
/// Command for removing an item from a sale
/// </summary>
public record RemoveSaleItemCommand : IRequest<RemoveSaleItemResult>
{
    /// <summary>
    /// Gets the sale ID
    /// </summary>
    public Guid SaleId { get; }
    
    /// <summary>
    /// Gets the sale item ID to remove
    /// </summary>
    public Guid SaleItemId { get; }

    /// <summary>
    /// Initializes a new instance of RemoveSaleItemCommand
    /// </summary>
    /// <param name="saleId">The ID of the sale</param>
    /// <param name="saleItemId">The ID of the sale item to remove</param>
    public RemoveSaleItemCommand(Guid saleId, Guid saleItemId)
    {
        SaleId = saleId;
        SaleItemId = saleItemId;
    }
} 