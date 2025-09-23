using FCG.Domain.Events.Wallet;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FCG.Application.EventsHandlers.Wallet
{
    public class WalletCreatedEventHandler : INotificationHandler<WalletCreatedEvent>
    {
        private readonly ILogger<WalletCreatedEventHandler> _logger;

        public WalletCreatedEventHandler(ILogger<WalletCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(WalletCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[WalletCreatedEvent] Carteira criada: {WalletId} - {UserId} - {InitialBalance} - {OccurredOn}",
                notification.WalletId,
                notification.UserId,
                notification.InitialBalance,
                notification.OcurredOn);

            return Task.CompletedTask;
        }
    }
}
