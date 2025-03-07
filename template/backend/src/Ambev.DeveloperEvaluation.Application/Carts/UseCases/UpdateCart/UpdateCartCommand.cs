using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.UpdateCart;

public class UpdateCartCommand : IRequest<UpdateCartResult>
{
    public Guid Id { get; set; }
    public Guid CustomerExternalId { get; set; }
    public string CustomerName { get; set; }
    public Guid BranchExternalId { get; set; }
    public string BranchName { get; set; }
    public List<UpdateCartProductCommand> Products { get; set; }
}

public class UpdateCartProductCommand
{
    public Guid ProductExternalId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
