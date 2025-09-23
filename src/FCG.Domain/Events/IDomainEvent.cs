using MediatR;

namespace FCG.Domain.Events
{
    public interface IDomainEvent : INotification
    {
        DateTime OcurredOn { get; }
    }
}
