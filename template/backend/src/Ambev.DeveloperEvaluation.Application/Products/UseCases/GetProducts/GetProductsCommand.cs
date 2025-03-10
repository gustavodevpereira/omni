using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;

/// <summary>
/// Command for retrieving all products with pagination
/// </summary>
public record GetProductsCommand : IRequest<GetProductsResult>
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
    /// Initializes a new instance of GetProductsCommand
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1)</param>
    /// <param name="pageSize">The number of items per page</param>
    public GetProductsCommand(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
} 