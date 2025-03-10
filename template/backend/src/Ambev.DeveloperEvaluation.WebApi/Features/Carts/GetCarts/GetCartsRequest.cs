using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

/// <summary>
/// Request model for retrieving a paginated list of carts.
/// </summary>
/// <remarks>
/// Inherits pagination parameters (PageNumber and PageSize) from <see cref="PaginationRequest"/>.
/// The current user's ID will be automatically applied as a filter from the authentication context.
/// </remarks>
public class GetCartsRequest : PaginationRequest
{
    // Inherits PageNumber and PageSize from PaginationRequest
} 