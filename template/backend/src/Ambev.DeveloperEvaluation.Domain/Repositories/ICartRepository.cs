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
    /// Gets a cart by its ID
    /// </summary>
    /// <param name="id">The cart ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart if found, otherwise null</returns>
    Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all carts
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>All carts</returns>
    Task<IEnumerable<Cart>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets paged carts
    /// </summary>
    /// <param name="pageNumber">The page number</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged carts</returns>
    Task<IEnumerable<Cart>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the total count of carts
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The total count of carts</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a cart
    /// </summary>
    /// <param name="cart">The cart to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated cart</returns>
    Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellationToken = default);
}
