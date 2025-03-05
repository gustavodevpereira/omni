namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Represents the result of canceling a sale.
/// </summary>
public class CancelSaleResult
{
    /// <summary>
    /// Gets or sets the identifier of the canceled sale.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Indicates whether the sale was successfully canceled.
    /// </summary>
    public bool IsCancelled { get; set; }
}
