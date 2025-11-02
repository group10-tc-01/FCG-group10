using FCG.Domain.Enum;

namespace FCG.Domain.Events.Game
{
    public record GameCreatedEvent(Guid GameId, string GameName, GameCategory Category) : DomainEvent;
}
