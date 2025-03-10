namespace Ambev.DeveloperEvaluation.Application.Common.Commands;

/// <summary>
/// Base abstract record for paginated requests.
/// Provides common pagination parameters and validation for all paginated requests in the application.
/// </summary>
/// <remarks>
/// This base class encapsulates the common pagination logic to eliminate duplication across different request types.
/// All paginated commands should inherit from this base class rather than implementing pagination parameters independently.
/// </remarks>
public abstract record PaginatedRequestBase
{
    /// <summary>
    /// Gets the page number for pagination (1-based).
    /// </summary>
    /// <remarks>
    /// Page numbers start at 1 for the first page. This provides a more intuitive interface for API consumers.
    /// </remarks>
    public int PageNumber { get; }

    /// <summary>
    /// Gets the page size (number of items per page) for pagination.
    /// </summary>
    /// <remarks>
    /// Defines how many records should be returned per page. This is used to calculate the appropriate
    /// SQL OFFSET/LIMIT values when querying the database.
    /// </remarks>
    public int PageSize { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginatedRequestBase"/> class with specified pagination parameters.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1)</param>
    /// <param name="pageSize">The number of items per page</param>
    protected PaginatedRequestBase(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize < 1 ? 10 : pageSize;
    }
} 