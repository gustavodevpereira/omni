using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;
using Ambev.DeveloperEvaluation.Application.Common.Results;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;

/// <summary>
/// Response containing a paginated list of carts.
/// </summary>
/// <remarks>
/// This result is returned by the <see cref="GetCartsHandler"/> and contains
/// a collection of carts along with pagination metadata.
/// </remarks>
public record GetCartsResult : PaginatedResultBase<CartResult>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetCartsResult"/> class.
    /// </summary>
    public GetCartsResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCartsResult"/> class with specified data.
    /// </summary>
    /// <param name="items">The collection of cart results</param>
    /// <param name="totalCount">The total count of all carts (before pagination)</param>
    /// <param name="pageNumber">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public GetCartsResult(IReadOnlyCollection<CartResult> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
