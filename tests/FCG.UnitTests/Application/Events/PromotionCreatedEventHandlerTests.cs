using FCG.Application.EventsHandlers.Promotion;
using FCG.Domain.Events.Promotion;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.Events
{
    public class PromotionCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenPromotionCreatedEventIsPublished()
        {
            var loggerMock = new Mock<ILogger<PromotionCreatedEventHandler>>();
            var handler = new PromotionCreatedEventHandler(loggerMock.Object);

            var promotionId = Guid.NewGuid();
            var gameId = Guid.NewGuid();

            var @event = new PromotionCreatedEvent(promotionId, gameId);

            await handler.Handle(@event, CancellationToken.None);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        v.ToString()!.Contains(promotionId.ToString()) &&
                        v.ToString()!.Contains(gameId.ToString())),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}
