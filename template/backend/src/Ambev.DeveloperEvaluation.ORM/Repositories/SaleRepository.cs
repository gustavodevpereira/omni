using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Sale?> GetSaleByIdAsync(int saleId, CancellationToken cancellationToken)
        {
            return await _context.Sales.FindAsync([saleId], cancellationToken);
        }

        public async Task UpdateSaleAsync(Sale sale, CancellationToken cancellationToken)
        {
            _context.Sales.Update(sale);
            await Task.CompletedTask;
        }
    }
}
