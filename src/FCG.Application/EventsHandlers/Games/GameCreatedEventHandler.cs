using FCG.Domain.Events.Game;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FCG.Application.EventsHandlers.Games
{
    public class GameCreatedEventHandler : INotificationHandler<GameCreatedEvent>
    {
        private readonly ILogger<GameCreatedEventHandler> _logger;

        public GameCreatedEventHandler(ILogger<GameCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(GameCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[GameCreatedEvent] Jogo criado: {GameId} - {GameName} - {Category} - {OccurredOn}",
                notification.GameId,
                notification.GameName,
                notification.Category,
                notification.OcurredOn);

            return Task.CompletedTask;
        }
    }
}
