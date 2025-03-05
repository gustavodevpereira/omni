using Ambev.DeveloperEvaluation.Domain.Entities.Sales;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> GetSaleByIdAsync(int saleId, CancellationToken cancellationToken);
        Task UpdateSaleAsync(Sale sale, CancellationToken cancellationToken);
    }
}
