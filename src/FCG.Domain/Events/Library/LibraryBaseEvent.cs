
namespace FCG.Domain.Events.Library
{
    public class LibraryBaseEvent : IDomainEvent
    {
        public Guid LibraryId { get; set; }
        public DateTime OcurredOn { get; set; }

        public LibraryBaseEvent(Guid libraryId)
        {
            LibraryId = libraryId;
            OcurredOn = DateTime.Now;
        }
    }
}
