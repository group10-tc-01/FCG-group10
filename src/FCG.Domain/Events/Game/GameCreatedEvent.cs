namespace FCG.Domain.Events.Game
{
    public record GameCreatedEvent(Guid GameId, string GameName,
         string Category) : DomainEvent
    {
    }

}
