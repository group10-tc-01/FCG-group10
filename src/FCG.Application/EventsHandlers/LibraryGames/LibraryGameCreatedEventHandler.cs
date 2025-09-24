using FCG.Domain.Events.LibraryGame;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FCG.Application.EventsHandlers.LibraryGames
{
    public class LibraryGameCreatedEventHandler : INotificationHandler<LibraryGameCreatedEvent>
    {
        private readonly ILogger<LibraryGameCreatedEventHandler> _logger;

        public LibraryGameCreatedEventHandler(ILogger<LibraryGameCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(LibraryGameCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[LibraryGameCreatedEvent] Jogo adicionado à biblioteca: {LibraryGameId} - {LibraryId} - {GameId} - {OccurredOn}",
                notification.LibraryGameId,
                notification.LibraryId,
                notification.GameId,
                notification.OcurredOn);

            return Task.CompletedTask;
        }
    }
}
