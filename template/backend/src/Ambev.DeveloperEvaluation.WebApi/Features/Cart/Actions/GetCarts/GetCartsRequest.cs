namespace Ambev.DeveloperEvaluation.WebApi.Features.Cart.Actions.GetCarts;

/// <summary>
/// Request for getting a paginated list of carts
/// </summary>
public class GetCartsRequest
{
    /// <summary>
    /// Page number for pagination
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Ordering of results (e.g., "id desc, userId asc")
    /// </summary>
    public string? Order { get; set; }
}