using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.UseCases.GetCarts;

/// <summary>
/// Command for retrieving carts with pagination
/// </summary>
public record GetCartsCommand : IRequest<GetCartsResult>
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
    /// Gets the customer ID to filter carts
    /// </summary>
    public Guid? CustomerId { get; }

    /// <summary>
    /// Initializes a new instance of GetCartsCommand
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1)</param>
    /// <param name="pageSize">The number of items per page</param>
    public GetCartsCommand(int pageNumber = 1, int pageSize = 10)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        CustomerId = null;
    }

    /// <summary>
    /// Initializes a new instance of GetCartsCommand with customer filter
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1)</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <param name="customerId">The customer ID to filter carts</param>
    public GetCartsCommand(int pageNumber, int pageSize, Guid customerId)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        CustomerId = customerId;
    }
}