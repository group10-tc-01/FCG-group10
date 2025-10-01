using FCG.Application.EventsHandlers.LibraryGames;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Events;
using FCG.Domain.Events.LibraryGame;
using FluentAssertions;

namespace FCG.UnitTests.Application.Events
{
    public class LibraryGameCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenLibraryGameCreatedEventIsPublished()
        {
            var loggerBuilder = new LoggerMockBuilder<LibraryGameCreatedEventHandler>().CaptureInformationLog();
            var handler = new LibraryGameCreatedEventHandler(loggerBuilder.Build().Object);

            var libraryGame = LibraryGameBuilder.Build();

            var @event = new LibraryGameCreatedEvent(libraryGame.Id, libraryGame.LibraryId, libraryGame.GameId);

            await handler.Handle(@event, CancellationToken.None);

            var loggedMessage = loggerBuilder.GetLoggedMessage();
            loggedMessage.Should().NotBeNull("uma mensagem de log deve ter sido capturada");

            loggedMessage.Should().Contain(libraryGame.Id.ToString());
            loggedMessage.Should().Contain(libraryGame.LibraryId.ToString());
            loggedMessage.Should().Contain(libraryGame.GameId.ToString());
        }
    }
}
