namespace Ambev.DeveloperEvaluation.WebApi.Common;

/// <summary>
/// Base request for pagination
/// </summary>
public class PaginationRequest
{
    /// <summary>
    /// The page number (starting from 1)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// The number of items per page
    /// </summary>
    public int PageSize { get; set; } = 10;
} 