using FCG.Application.EventsHandlers.Promotion;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Events;
using FCG.Domain.Events.Promotion;
using FluentAssertions;

namespace FCG.UnitTests.Application.Events
{
    public class PromotionCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenPromotionCreatedEventIsPublished()
        {
            var loggerBuilder = new LoggerMockBuilder<PromotionCreatedEventHandler>().CaptureInformationLog();
            var handler = new PromotionCreatedEventHandler(loggerBuilder.Build().Object);

            var promotion = PromotionBuilder.Build();

            var @event = new PromotionCreatedEvent(promotion.Id, promotion.GameId);

            await handler.Handle(@event, CancellationToken.None);

            var loggedMessage = loggerBuilder.GetLoggedMessage();
            loggedMessage.Should().NotBeNull("uma mensagem de log deve ter sido capturada");

            loggedMessage.Should().Contain(promotion.Id.ToString());
            loggedMessage.Should().Contain(promotion.GameId.ToString());
        }
    }
}
