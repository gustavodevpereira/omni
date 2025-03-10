namespace Ambev.DeveloperEvaluation.Application.Common.Results;

/// <summary>
/// Base abstract record for paginated results.
/// Provides common pagination metadata and collection properties for all paginated responses in the application.
/// </summary>
/// <typeparam name="T">The type of items in the paginated collection</typeparam>
/// <remarks>
/// This base class encapsulates the common pagination response structure to eliminate duplication across different result types.
/// All paginated results should inherit from this base class rather than implementing pagination properties independently.
/// </remarks>
public abstract record PaginatedResultBase<T>
{
    /// <summary>
    /// Gets the collection of items for the current page.
    /// </summary>
    public IReadOnlyCollection<T> Items { get; init; } = new List<T>();

    /// <summary>
    /// Gets the total count of items across all pages.
    /// </summary>
    /// <remarks>
    /// This value represents the count of all items that match the query before pagination was applied.
    /// </remarks>
    public int TotalCount { get; init; }

    /// <summary>
    /// Gets the current page number (1-based).
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Gets the page size (number of items per page).
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total number of pages based on the total count and page size.
    /// </summary>
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;

    /// <summary>
    /// Gets a value indicating whether there is a previous page available.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Gets a value indicating whether there is a next page available.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
} 