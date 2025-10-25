namespace FCG.Domain.Events.UserGame
{
    public record UserGameCreatedEvent(
        Guid UserId,
        Guid GameId
    ) : DomainEvent;
}
