using FCG.Domain.Events.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FCG.Application.EventsHandlers.Users
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {

        private readonly ILogger<UserCreatedEventHandler> _logger;

        public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[UserCreatedEvent] Usuário criado: {UserId} - {Name} - {Email} - {OccurredOn}",
                notification.UserId,
                notification.Name,
                notification.Email,
                notification.OcurredOn);

            return Task.CompletedTask;
        }
    }
}
