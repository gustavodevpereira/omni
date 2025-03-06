namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Response model after creating a new sale
/// </summary>
public class CreateSaleResponse
{
    /// <summary>
    /// The unique identifier of the sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The date when the sale was made
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The external customer ID
    /// </summary>
    public string CustomerExternalId { get; set; } = string.Empty;

    /// <summary>
    /// The customer name
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// The external branch ID
    /// </summary>
    public string BranchExternalId { get; set; } = string.Empty;

    /// <summary>
    /// The branch name
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// The status of the sale
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// The total amount of the sale
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The list of sale items
    /// </summary>
    public List<SaleItemResponse> Items { get; set; } = new List<SaleItemResponse>();
}

/// <summary>
/// Response model for a sale item
/// </summary>
public class SaleItemResponse
{
    /// <summary>
    /// The unique identifier of the sale item
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The external product ID
    /// </summary>
    public string ProductExternalId { get; set; } = string.Empty;

    /// <summary>
    /// The product name
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// The quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The discount percentage applied
    /// </summary>
    public decimal DiscountPercentage { get; set; }

    /// <summary>
    /// The total amount for this item
    /// </summary>
    public decimal TotalAmount { get; set; }
} 