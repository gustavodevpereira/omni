using Ambev.DeveloperEvaluation.Domain.Entities.Products;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Product entity operations
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Gets paged products
    /// </summary>
    /// <param name="pageNumber">The page number</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged products</returns>
    Task<IEnumerable<Product>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the total count of products
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The total count of products</returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);
} 