using Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.GetCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.GetCarts;

/// <summary>
/// Response containing paginated list of carts
/// </summary>
public class GetCartsResponse
{
    /// <summary>
    /// List of cart items
    /// </summary>
    public List<GetCartResponse> Data { get; set; } = [];

    /// <summary>
    /// Total number of items across all pages
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }
}