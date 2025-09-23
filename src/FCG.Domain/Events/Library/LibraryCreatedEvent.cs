namespace FCG.Domain.Events.Library
{
    public class LibraryCreatedEvent : LibraryBaseEvent
    {
        public Guid UserId { get; set; }

        public LibraryCreatedEvent(Guid libraryId, Guid userId) : base(libraryId)
        {
            UserId = userId;
        }
    }
}
