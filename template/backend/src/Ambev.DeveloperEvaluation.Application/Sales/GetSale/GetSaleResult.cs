namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Represents the response returned after successfully retrieving a sale.
/// </summary>
public class GetSaleResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }
    
    /// <summary>
    /// Gets or sets the external identifier for the customer.
    /// </summary>
    public string CustomerExternalId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the external identifier for the branch.
    /// </summary>
    public string BranchExternalId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the branch name.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the status of the sale.
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the total amount of the sale.
    /// </summary>
    public decimal TotalAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the items in the sale.
    /// </summary>
    public List<SaleItemDto> Items { get; set; } = new List<SaleItemDto>();

    /// <summary>
    /// Represents an item in a sale.
    /// </summary>
    public class SaleItemDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale item.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Gets or sets the external product identifier.
        /// </summary>
        public string ProductExternalId { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public int Quantity { get; set; }
        
        /// <summary>
        /// Gets or sets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }
        
        /// <summary>
        /// Gets or sets the discount percentage applied to this sale item.
        /// </summary>
        public decimal DiscountPercentage { get; set; }
        
        /// <summary>
        /// Gets or sets the total amount for this sale item after discount.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
} 