namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// API response model for the GetSale operation.
    /// </summary>
    public class GetSaleResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who made the sale.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the date when the sale was made.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the list of sale items included in the sale.
        /// </summary>
        public List<GetSaleItem> SaleItems { get; set; } = [];
    }

    /// <summary>
    /// API response model for a sale item.
    /// </summary>
    public class GetSaleItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the external product ID.
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
        /// Gets or sets the discount percentage applied.
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the total amount for this sale item.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
