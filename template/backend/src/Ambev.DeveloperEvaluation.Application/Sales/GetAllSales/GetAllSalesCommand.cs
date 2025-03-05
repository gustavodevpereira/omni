using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;

/// <summary>
/// Command for retrieving all sales with pagination
/// </summary>
public record GetAllSalesCommand : IRequest<GetAllSalesResult>
{
    /// <summary>
    /// Gets the page number for pagination
    /// </summary>
    public int PageNumber { get; }
    
    /// <summary>
    /// Gets the page size for pagination
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Initializes a new instance of GetAllSalesCommand
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1)</param>
    /// <param name="pageSize">The number of items per page</param>
    public GetAllSalesCommand(int pageNumber = 1, int pageSize = 10)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
} 