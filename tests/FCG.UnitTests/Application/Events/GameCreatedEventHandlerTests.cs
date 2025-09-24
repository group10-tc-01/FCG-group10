using FCG.Application.EventsHandlers.Games;
using FCG.Domain.Events.Game;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.Events
{
    public class GameCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenGameCreatedEventIsPublished()
        {
            var loggerMock = new Mock<ILogger<GameCreatedEventHandler>>();
            var handler = new GameCreatedEventHandler(loggerMock.Object);

            var gameId = Guid.NewGuid();
            var gameName = "The Witcher 3";
            var category = "RPG";

            var @event = new GameCreatedEvent(gameId, gameName, category);

            await handler.Handle(@event, CancellationToken.None);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        v.ToString()!.Contains(gameId.ToString()) &&
                        v.ToString()!.Contains(gameName) &&
                        v.ToString()!.Contains(category)),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}
