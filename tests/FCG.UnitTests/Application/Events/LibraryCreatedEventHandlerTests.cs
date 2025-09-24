using FCG.Application.EventsHandlers.Librarys;
using FCG.Domain.Events.Library;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.Events
{
    public class LibraryCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenLibraryCreatedEventIsPublished()
        {
            var loggerMock = new Mock<ILogger<LibraryCreatedEventHandler>>();
            var handler = new LibraryCreatedEventHandler(loggerMock.Object);

            var libraryId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var @event = new LibraryCreatedEvent(libraryId, userId);

            await handler.Handle(@event, CancellationToken.None);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        v.ToString()!.Contains(libraryId.ToString()) &&
                        v.ToString()!.Contains(userId.ToString())),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}
