namespace FCG.Domain.Events.Library
{
    public record LibraryCreatedEvent(Guid LibraryId, Guid UserId) : DomainEvent;
}
