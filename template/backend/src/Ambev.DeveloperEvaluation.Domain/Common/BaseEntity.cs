using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Common
{
    public class BaseEntity : IComparable<BaseEntity>
    {
        private readonly List<INotification> _domainEvents = new();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public Guid Id { get; set; }

        public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
        {
            return Validator.ValidateAsync(this);
        }

        public int CompareTo(BaseEntity? other)
        {
            if (other == null)
            {
                return 1;
            }

            return other.Id.CompareTo(Id);
        }

        protected void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
