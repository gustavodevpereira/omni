namespace Ambev.DeveloperEvaluation.Application.Common.Pagination;

/// <summary>
/// Base class for paginated results
/// </summary>
/// <typeparam name="T">The type of items in the result</typeparam>
public class PaginatedResult<T>
{
    /// <summary>
    /// List of items in the current page
    /// </summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the total count of items.
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