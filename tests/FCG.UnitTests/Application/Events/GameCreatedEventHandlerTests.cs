using FCG.Application.EventsHandlers.Games;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Events;
using FCG.Domain.Events.Game;
using FluentAssertions;

namespace FCG.UnitTests.Application.Events
{
    public class GameCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenGameCreatedEventIsPublished()
        {
            var loggerBuilder = new LoggerMockBuilder<GameCreatedEventHandler>().CaptureInformationLog();
            var handler = new GameCreatedEventHandler(loggerBuilder.Build().Object);

            var game = GameBuilder.Build();

            var @event = new GameCreatedEvent(game.Id, game.Name, game.Category);

            await handler.Handle(@event, CancellationToken.None);

            var loggedMessage = loggerBuilder.GetLoggedMessage();
            loggedMessage.Should().NotBeNull("uma mensagem de log deve ter sido capturada");

            loggedMessage.Should().Contain(game.Id.ToString());
            loggedMessage.Should().Contain(game.Name);
            loggedMessage.Should().Contain(game.Category);
        }
    }
}
