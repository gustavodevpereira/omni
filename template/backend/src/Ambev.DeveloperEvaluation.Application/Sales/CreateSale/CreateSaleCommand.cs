using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Command for creating a new sale.
    /// </summary>
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string CustomerExternalId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string BranchExternalId { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;

        // Lista de itens da venda
        public List<CreateSaleItemCommand> Items { get; set; } = new List<CreateSaleItemCommand>();
    }

    /// <summary>
    /// Command for creating a sale item.
    /// </summary>
    public class CreateSaleItemCommand
    {
        public string ProductExternalId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
