using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.AddCart;

/// <summary>
/// Command to add a new cart
/// </summary>
public class AddCartCommand : IRequest<CartResult>
{
    /// <summary>
    /// Costumer identifier who owns the cart
    /// </summary>
    public Guid CostumerId { get; set; }

    /// <summary>
    /// Costumer name who owns the cart
    /// </summary>
    public string CostumerName { get; set; }

    /// <summary>
    /// Branch identifier where the sale was made
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Branch name where the sale was made
    /// </summary>
    public string BranchName { get; set; }

    /// <summary>
    /// Cart creation date
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Products in the cart
    /// </summary>
    public List<AddCartProductCommand> Products { get; set; } = [];
}

/// <summary>
/// Product command to be added to a cart
/// </summary>
public class AddCartProductCommand
{
    /// <summary>
    /// Product identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Product identifier
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public decimal UnitPrice { get; set; }

}
