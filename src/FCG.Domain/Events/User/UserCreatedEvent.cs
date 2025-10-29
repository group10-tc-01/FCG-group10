namespace FCG.Domain.Events.User
{
    public record UserCreatedEvent(Guid UserId, string Name, string Email) : DomainEvent;
}
