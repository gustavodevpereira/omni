using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Cart operations
/// </summary>
public interface ICartRepository
{
    /// <summary>
    /// Creates a new cart
    /// </summary>
    /// <param name="cart">The cart to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart</returns>
    Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets paged carts for a specific customer
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <param name="pageNumber">The page number</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged carts for the customer</returns>
    Task<IEnumerable<Cart>> GetAllPagedByCustomerAsync(Guid customerId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the total count of carts for a specific customer
    /// </summary>
    /// <param name="customerId">The customer ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The total count of carts for the customer</returns>
    Task<int> CountByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
}
