using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of the IProductRepository interface.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly DefaultContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the ProductRepository class.
    /// </summary>
    /// <param name="dbContext">The database context</param>
    public ProductRepository(DefaultContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Gets paged products
    /// </summary>
    /// <param name="pageNumber">The page number</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged products</returns>
    public async Task<IEnumerable<Product>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the total count of products
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The total count of products</returns>
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.CountAsync(cancellationToken);
    }
} 