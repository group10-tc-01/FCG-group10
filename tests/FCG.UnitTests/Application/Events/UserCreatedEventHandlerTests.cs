using FCG.Application.EventsHandlers.Users;
using FCG.Domain.Events.User;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.Events
{
    public class UserCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenEventIsPublished()
        {
            var loggerMock = new Mock<ILogger<UserCreatedEventHandler>>();
            var handler = new UserCreatedEventHandler(loggerMock.Object);

            var userId = Guid.NewGuid();
            var name = "lohhan";
            var email = "test@example.com";
            var occurredOn = DateTime.UtcNow;

            var @event = new UserCreatedEvent(userId, name, email);

            await handler.Handle(@event, CancellationToken.None);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(userId.ToString())
                                                  && v.ToString()!.Contains(name)
                                                  && v.ToString()!.Contains(email)),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}
