namespace FCG.Domain.Events
{
    public abstract record DomainEvent : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OcurredOn { get; init; } = DateTime.UtcNow;
    }
}
