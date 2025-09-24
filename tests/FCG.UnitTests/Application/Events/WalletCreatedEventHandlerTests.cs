using FCG.Application.EventsHandlers.Wallet;
using FCG.Domain.Events.Wallet;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.Events
{
    public class WalletCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenWalletCreatedEventIsPublished()
        {
            var loggerMock = new Mock<ILogger<WalletCreatedEventHandler>>();
            var handler = new WalletCreatedEventHandler(loggerMock.Object);

            var walletId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var @event = new WalletCreatedEvent(walletId, userId);

            await handler.Handle(@event, CancellationToken.None);

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        v.ToString()!.Contains(walletId.ToString()) &&
                        v.ToString()!.Contains(userId.ToString())),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}
