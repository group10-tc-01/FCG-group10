using FCG.Application.EventsHandlers.LibraryGames;
using FCG.Domain.Events.LibraryGame;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.Events
{
    public class LibraryGameCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenLibraryGameCreatedEventIsPublished()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<LibraryGameCreatedEventHandler>>();
            var handler = new LibraryGameCreatedEventHandler(loggerMock.Object);

            var libraryGameId = Guid.NewGuid();
            var libraryId = Guid.NewGuid();
            var gameId = Guid.NewGuid();

            var @event = new LibraryGameCreatedEvent(libraryGameId, libraryId, gameId);

            // Act
            await handler.Handle(@event, CancellationToken.None);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        v.ToString()!.Contains(libraryGameId.ToString()) &&
                        v.ToString()!.Contains(libraryId.ToString()) &&
                        v.ToString()!.Contains(gameId.ToString())
                        ),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}
