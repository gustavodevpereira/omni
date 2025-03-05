using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM
{
    /// <summary>
    /// Implements the Unit of Work pattern using Entity Framework Core.
    /// This class coordinates database transactions and dispatches domain events via MediatR.
    /// 
    /// Note: Repositories should only perform data modifications without calling SaveChangesAsync,
    /// as the UnitOfWork handles saving changes and dispatching domain events.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The EF Core database context (e.g. DefaultContext).</param>
        /// <param name="mediator">The mediator used to publish domain events.</param>
        public UnitOfWork(DbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            // Save all changes made in the context to the database.
            int result = await _context.SaveChangesAsync(cancellationToken);

            // After a successful commit, dispatch all accumulated domain events.
            await DispatchDomainEvents(cancellationToken);

            return result;
        }

        /// <summary>
        /// Dispatches domain events collected by the tracked entities.
        /// This ensures that domain events are only published after the database transaction completes successfully.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the dispatch operation.</param>
        /// <returns>A task representing the asynchronous dispatch process.</returns>
        private async Task DispatchDomainEvents(CancellationToken cancellationToken)
        {
            // Find all entities that have registered domain events.
            var domainEntities = _context.ChangeTracker
                .Entries<BaseEntity>()
                .Where(entry => entry.Entity.DomainEvents.Any())
                .Select(entry => entry.Entity)
                .ToList();

            // Collect all domain events from these entities.
            var domainEvents = domainEntities
                .SelectMany(entity => entity.DomainEvents)
                .ToList();

            // Clear domain events from the entities to prevent duplicate dispatch.
            domainEntities.ForEach(entity => entity.ClearDomainEvents());

            // Publish each domain event using MediatR.
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
