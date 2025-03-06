using MediatR;
using Ambev.DeveloperEvaluation.Application.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Command for creating a new sale.
    /// Implements IUserCommand to support automatic user information population.
    /// </summary>
    public class CreateSaleCommand : IRequest<CreateSaleResult>, IUserCommand
    {
        /// <summary>
        /// Gets or sets the sale number.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sale date.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// Gets or sets the external identifier of the customer.
        /// </summary>  
        public string CustomerExternalId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the branch.
        /// </summary>
        public string BranchName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the external identifier of the branch.
        /// </summary>  
        public string BranchExternalId { get; set; } = string.Empty;

        // Lista de itens da venda
        public List<CreateSaleItemCommand> Items { get; set; } = new List<CreateSaleItemCommand>();
    }

    /// <summary>
    /// Command for creating a sale item.
    /// </summary>
    public class CreateSaleItemCommand
    {
        /// <summary>
        /// Gets or sets the external identifier of the product.
        /// </summary>
        public string ProductExternalId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the product.
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
    }
}
