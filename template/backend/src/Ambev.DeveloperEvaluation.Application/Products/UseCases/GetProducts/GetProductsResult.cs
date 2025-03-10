using Ambev.DeveloperEvaluation.Application.Common.Results;
using Ambev.DeveloperEvaluation.Application.Products.Common.Results;

namespace Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;

/// <summary>
/// Response containing a paginated list of products.
/// </summary>
/// <remarks>
/// This result is returned by the <see cref="GetProductsHandler"/> and contains
/// a collection of products along with pagination metadata.
/// </remarks>
public record GetProductsResult : PaginatedResultBase<ProductResult>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductsResult"/> class.
    /// </summary>
    public GetProductsResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductsResult"/> class with specified data.
    /// </summary>
    /// <param name="items">The collection of product results</param>
    /// <param name="totalCount">The total count of all products (before pagination)</param>
    /// <param name="pageNumber">The current page number</param>
    /// <param name="pageSize">The page size</param>
    public GetProductsResult(IReadOnlyCollection<ProductResult> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
} 