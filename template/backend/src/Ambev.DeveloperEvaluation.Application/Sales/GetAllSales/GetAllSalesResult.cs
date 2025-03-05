namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;

/// <summary>
/// Represents the response returned after retrieving all sales.
/// </summary>
public class GetAllSalesResult
{
    /// <summary>
    /// Gets or sets the list of sales.
    /// </summary>
    public List<SaleDto> Items { get; set; } = new List<SaleDto>();
    
    /// <summary>
    /// Gets or sets the total count of sales.
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int PageNumber { get; set; }
    
    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; }
    
    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Represents a sale summary in the list of sales.
    /// </summary>
    public class SaleDto
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
        /// Gets or sets the customer name.
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the branch name.
        /// </summary>
        public string BranchName { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the total amount of the sale.
        /// </summary>
        public decimal TotalAmount { get; set; }
        
        /// <summary>
        /// Gets or sets the status of the sale.
        /// </summary>
        public string Status { get; set; } = string.Empty;
        
        /// <summary>
        /// Gets or sets the number of items in the sale.
        /// </summary>
        public int ItemCount { get; set; }
    }
} 