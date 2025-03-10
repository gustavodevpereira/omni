using Ambev.DeveloperEvaluation.Application.Common.Commands;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UseCases.GetProducts;

/// <summary>
/// Command for retrieving all products with pagination.
/// </summary>
/// <remarks>
/// This command is used to request a paginated list of products from the system.
/// It inherits pagination functionality from <see cref="PaginatedRequestBase"/>.
/// </remarks>
public record GetProductsCommand : PaginatedRequestBase, IRequest<GetProductsResult>
{
    /// <summary>
    /// Initializes a new instance of <see cref="GetProductsCommand"/> with pagination parameters.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1)</param>
    /// <param name="pageSize">The number of items per page</param>
    public GetProductsCommand(int pageNumber = 1, int pageSize = 10) 
        : base(pageNumber, pageSize)
    {
    }
} 