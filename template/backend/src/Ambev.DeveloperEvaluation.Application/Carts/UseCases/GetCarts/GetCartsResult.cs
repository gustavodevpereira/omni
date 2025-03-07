using Ambev.DeveloperEvaluation.Application.Carts.Common.Results;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;

/// <summary>
/// Response for list of carts
/// </summary>
public class GetCartsResult
{
    /// <summary>
    /// List of individual carts
    /// </summary>
    public List<CartResult> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the total count of carts.
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
