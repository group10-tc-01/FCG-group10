using System.Collections.ObjectModel;

namespace FCG.Domain.Entities
{
    public abstract class BaseEntity
    {
        private readonly List<object> _domainEvents = new();

        protected BaseEntity() { }

        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; protected set; }
        public bool IsActive { get; protected set; } = true;

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public IReadOnlyCollection<object> DomainEvents => new ReadOnlyCollection<object>(_domainEvents);

        public void AddDomainEvent(object domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

    }
}
