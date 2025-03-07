namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.GetCart;

/// <summary>
/// Response containing cart details
/// </summary>
public class GetCartResponse
{
    /// <summary>
    /// Cart identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User identifier who owns the cart
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Cart creation date
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Products in the cart
    /// </summary>
    public List<CartProductResponse> Products { get; set; } = [];
}

/// <summary>
/// Product details in a cart
/// </summary>
public class CartProductResponse
{
    /// <summary>
    /// Product identifier
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity of the product
    /// </summary>
    public int Quantity { get; set; }
}