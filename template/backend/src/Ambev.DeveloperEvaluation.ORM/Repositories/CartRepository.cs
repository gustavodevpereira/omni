using Ambev.DeveloperEvaluation.Domain.Entities.Carts;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// Repository implementation for Cart operations
    /// </summary>
    public class CartRepository : ICartRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of CartRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public CartRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new Cart
        /// </summary>
        public async Task<Cart> CreateAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            await _context.Carts.AddAsync(cart, cancellationToken);
            return cart;
        }


        /// <summary>
        /// Gets paged carts for a specific customer
        /// </summary>
        public async Task<IEnumerable<Cart>> GetAllPagedByCustomerAsync(Guid customerId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Carts
                .Include(s => s.Products)
                .Where(c => c.CustomerExternalId == customerId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the total count of carts for a specific customer
        /// </summary>
        public async Task<int> CountByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await _context.Carts
                .Where(c => c.CustomerExternalId == customerId)
                .CountAsync(cancellationToken);
        }
    }
}
