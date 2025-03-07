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
        /// Gets a cart by its ID
        /// </summary>
        public async Task<Cart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Carts
                .Include(s => s.Products)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        /// <summary>
        /// Gets all cart
        /// </summary>
        public async Task<IEnumerable<Cart>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Carts
                .Include(s => s.Products)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets paged cart
        /// </summary>
        public async Task<IEnumerable<Cart>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Carts
                .Include(s => s.Products)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the total count of cart
        /// </summary>
        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Carts.CountAsync(cancellationToken);
        }

        /// <summary>
        /// Updates a cart
        /// </summary>
        public async Task<Cart> UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            _context.Entry(cart).State = EntityState.Modified;
            return cart;
        }
    }
}
