namespace FCG.Domain.Events.Game
{
    public record GameCreatedEvent(Guid GameId, string GameName,
        decimal Price, string Category) : DomainEvent
    {
    }

}
