using Ambev.DeveloperEvaluation.Application.Common.Commands;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;

/// <summary>
/// Command for retrieving carts with pagination and optional customer filtering.
/// </summary>
/// <remarks>
/// This command is used to request a paginated list of carts from the system, with an optional
/// filter by customer ID. It inherits pagination functionality from <see cref="PaginatedRequestBase"/>.
/// </remarks>
public record GetCartsCommand : PaginatedRequestBase, IRequest<GetCartsResult>
{
    /// <summary>
    /// Gets the customer ID to filter carts. If null, all carts will be retrieved.
    /// </summary>
    /// <remarks>
    /// When specified, only carts associated with the given customer will be returned.
    /// </remarks>
    public Guid CustomerId { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="GetCartsCommand"/> with pagination parameters.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1)</param>
    /// <param name="pageSize">The number of items per page</param>
    public GetCartsCommand(int pageNumber = 1, int pageSize = 10)
        : base(pageNumber, pageSize)
    {}

    /// <summary>
    /// Initializes a new instance of <see cref="GetCartsCommand"/> with pagination parameters and customer filter.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1)</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <param name="customerId">The customer ID to filter carts</param>
    public GetCartsCommand(int pageNumber, int pageSize, Guid customerId)
        : base(pageNumber, pageSize)
    {
        CustomerId = customerId;
    }
}