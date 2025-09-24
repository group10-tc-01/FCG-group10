using FCG.Domain.Events.Promotion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FCG.Application.EventsHandlers.Promotion
{
    public class PromotionCreatedEventHandler : INotificationHandler<PromotionCreatedEvent>
    {
        private readonly ILogger<PromotionCreatedEventHandler> _logger;

        public PromotionCreatedEventHandler(ILogger<PromotionCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(PromotionCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[PromotionCreatedEvent] Promoção criada: {PromotionId} - {GameId} - {OccurredOn}",
                notification.PromotionId,
                notification.GameId,
                notification.OcurredOn);

            return Task.CompletedTask;
        }
    }
}
