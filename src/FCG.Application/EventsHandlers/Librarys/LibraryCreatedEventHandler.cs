using FCG.Domain.Events.Library;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FCG.Application.EventsHandlers.Librarys
{
    public class LibraryCreatedEventHandler : INotificationHandler<LibraryCreatedEvent>
    {
        private readonly ILogger<LibraryCreatedEventHandler> _logger;

        public LibraryCreatedEventHandler(ILogger<LibraryCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(LibraryCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[LibraryCreatedEvent] Biblioteca criada: {LibraryId} - {UserId} - {OccurredOn}",
                notification.LibraryId,
                notification.UserId,
                notification.OcurredOn);

            return Task.CompletedTask;
        }
    }
}
