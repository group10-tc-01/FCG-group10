using MediatR;

namespace FCG.Domain.Events
{
    public interface IDomainEvent : INotification
    {
        Guid EventId { get; }
        DateTime OcurredOn { get; }
    }
}
