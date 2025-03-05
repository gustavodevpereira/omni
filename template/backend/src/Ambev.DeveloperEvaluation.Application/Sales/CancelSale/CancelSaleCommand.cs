using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Command for canceling (invalidating) an existing sale.
/// </summary>
public class CancelSaleCommand : IRequest<CancelSaleResult>
{
    /// <summary>
    /// Gets or sets the identifier of the sale to be canceled.
    /// </summary>
    public Guid SaleId { get; set; }
}
