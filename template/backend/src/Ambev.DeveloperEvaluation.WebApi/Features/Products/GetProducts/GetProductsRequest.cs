using Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

/// <summary>
/// Request model for retrieving products with pagination.
/// </summary>
/// <remarks>
/// Inherits pagination parameters (PageNumber and PageSize) from <see cref="PaginationRequest"/>.
/// </remarks>
public class GetProductsRequest : PaginationRequest
{
    // Inherits PageNumber and PageSize from PaginationRequest
} 