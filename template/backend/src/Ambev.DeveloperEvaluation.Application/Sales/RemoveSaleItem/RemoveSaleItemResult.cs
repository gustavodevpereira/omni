namespace Ambev.DeveloperEvaluation.Application.Sales.RemoveSaleItem;

/// <summary>
/// Represents the response returned after removing an item from a sale.
/// </summary>
public class RemoveSaleItemResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale.
    /// </summary>
    public Guid SaleId { get; set; }
    
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the unique identifier of the removed sale item.
    /// </summary>
    public Guid RemovedSaleItemId { get; set; }
    
    /// <summary>
    /// Gets or sets the new total amount of the sale after removing the item.
    /// </summary>
    public decimal NewTotalAmount { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Gets or sets a message describing the result of the operation.
    /// </summary>
    public string Message { get; set; } = string.Empty;
} 