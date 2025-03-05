namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Defines a contract for a Unit of Work which coordinates the writing out of changes and 
    /// dispatches domain events after successful transactions.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits all changes made within the unit of work to the database and dispatches any associated domain events.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the commit operation.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
