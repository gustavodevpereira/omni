namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddCart;

/// <summary>
/// Request model for creating a new cart
/// </summary>
public class AddCartRequest
{
    /// <summary>
    /// Branch identifier where the cart was created
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Branch name where the cart was created
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Products to be added to the cart
    /// </summary>
    public List<AddCartProductRequest> Products { get; set; } = [];
}

/// <summary>
/// Request model for adding a product to a cart
/// </summary>
public class AddCartProductRequest
{
    /// <summary>
    /// Product identifier
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Product name
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product
    /// </summary>
    public decimal UnitPrice { get; set; }
} 