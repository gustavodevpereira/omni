using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem
{
    /// <summary>
    /// Command for adding a new item to an existing sale.
    /// </summary>
    public class AddSaleItemCommand : IRequest<AddSaleItemResult>
    {
        /// <summary>
        /// Gets or sets the identifier of the sale to which the item will be added.
        /// </summary>
        public Guid SaleId { get; set; }

        /// <summary>
        /// Gets or sets the external identifier of the product.
        /// </summary>
        public string ProductExternalId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product name (denormalized).
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the quantity of the product. Must be between 1 and 20.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
