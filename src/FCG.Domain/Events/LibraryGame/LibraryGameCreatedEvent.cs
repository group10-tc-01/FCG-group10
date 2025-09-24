namespace FCG.Domain.Events.LibraryGame
{
    public record LibraryGameCreatedEvent(
        Guid LibraryGameId,
        Guid LibraryId,
        Guid GameId,
        decimal PurchasePrice
    ) : DomainEvent;
}
