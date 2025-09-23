using MediatR;

namespace FCG.Domain.Events.LibraryGame
{
    public class LibraryGameBaseEvent : INotification
    {
        public Guid LibraryGameId { get; set; }
        public DateTime OcurredOn { get; set; }

        public LibraryGameBaseEvent(Guid libraryGameId)
        {
            LibraryGameId = libraryGameId;
            OcurredOn = DateTime.Now;
        }
    }
}
