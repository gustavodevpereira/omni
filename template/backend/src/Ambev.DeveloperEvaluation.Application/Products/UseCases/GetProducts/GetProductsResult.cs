using Ambev.DeveloperEvaluation.Application.Products.Common.Results;

namespace Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;

/// <summary>
/// Response for list of products
/// </summary>
public class GetProductsResult
{
    /// <summary>
    /// List of individual products
    /// </summary>
    public List<ProductResult> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the total count of products.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
} 