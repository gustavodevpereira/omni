namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.UpdateCart;

public class UpdateCartResult
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid CustomerExternalId { get; set; }
    public string CustomerName { get; set; }
    public Guid BranchExternalId { get; set; }
    public string BranchName { get; set; }
    public string Status { get; set; }
    public decimal TotalAmount { get; set; }
    public List<UpdateCartProductResult> Products { get; set; }
}

public class UpdateCartProductResult
{
    public Guid Id { get; set; }
    public Guid ProductExternalId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal TotalAmount { get; set; }
}
