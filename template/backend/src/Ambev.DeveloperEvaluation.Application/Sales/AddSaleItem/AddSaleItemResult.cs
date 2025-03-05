using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSaleItem
{
    /// <summary>
    /// Represents the result of adding an item to a sale.
    /// </summary>
    public class AddSaleItemResult
    {
        /// <summary>
        /// Gets or sets the identifier of the sale.
        /// </summary>
        public Guid SaleId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the newly added sale item.
        /// </summary>
        public Guid SaleItemId { get; set; }

        /// <summary>
        /// Gets or sets the new total amount of the sale after adding the item.
        /// </summary>
        public decimal NewTotalAmount { get; set; }
    }
}
