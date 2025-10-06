using FCG.Application.EventsHandlers.Wallets;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Events;
using FCG.Domain.Events.Wallet;
using FluentAssertions;

namespace FCG.UnitTests.Application.Events
{
    public class WalletCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenWalletCreatedEventIsPublished()
        {
            var loggerBuilder = new LoggerMockBuilder<WalletCreatedEventHandler>().CaptureInformationLog();
            var handler = new WalletCreatedEventHandler(loggerBuilder.Build().Object);

            var wallet = WalletBuilder.Build();

            var @event = new WalletCreatedEvent(wallet.Id, wallet.UserId);

            await handler.Handle(@event, CancellationToken.None);

            var loggedMessage = loggerBuilder.GetLoggedMessage();
            loggedMessage.Should().NotBeNull("uma mensagem de log deve ter sido capturada");

            loggedMessage.Should().Contain(wallet.Id.ToString());
            loggedMessage.Should().Contain(wallet.UserId.ToString());
        }
    }
}
