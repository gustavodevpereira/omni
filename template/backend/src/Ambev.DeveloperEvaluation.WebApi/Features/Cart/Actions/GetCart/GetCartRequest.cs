namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.GetCart;

/// <summary>
/// Request for getting a specific cart
/// </summary>
public class GetCartRequest
{
    /// <summary>
    /// The ID of the cart to retrieve
    /// </summary>
    public Guid Id { get; set; }
}