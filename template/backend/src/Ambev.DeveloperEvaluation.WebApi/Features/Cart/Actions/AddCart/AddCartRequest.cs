namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.AddCart;

/// <summary>
/// Request for adding a new cart
/// </summary>
public class AddCartRequest
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
    public List<AddCartProductRequest> Products { get; set; } = new();
}

/// <summary>
/// Product to be added to a cart
/// </summary>
public class AddCartProductRequest
{
    /// <summary>
    /// Product identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }
}