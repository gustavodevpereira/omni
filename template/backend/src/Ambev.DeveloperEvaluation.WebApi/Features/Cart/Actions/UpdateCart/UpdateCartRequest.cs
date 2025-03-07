namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.UpdateCart;

public class UpdateCartRequest
{
    public Guid CustomerExternalId { get; set; }
    public string CustomerName { get; set; }
    public Guid BranchExternalId { get; set; }
    public string BranchName { get; set; }
    public List<UpdateCartProductRequest> Products { get; set; }
}

public class UpdateCartProductRequest
{
    public Guid ProductExternalId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
