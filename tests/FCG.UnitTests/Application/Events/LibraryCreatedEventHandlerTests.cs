using FCG.Application.EventsHandlers.Librarys;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Events;
using FCG.Domain.Events.Library;
using FluentAssertions;

namespace FCG.UnitTests.Application.Events
{
    public class LibraryCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenLibraryCreatedEventIsPublished()
        {
            var loggerBuilder = new LoggerMockBuilder<LibraryCreatedEventHandler>().CaptureInformationLog();
            var handler = new LibraryCreatedEventHandler(loggerBuilder.Build().Object);

            var library = LibraryBuilder.Build();

            var @event = new LibraryCreatedEvent(library.Id, library.UserId);

            await handler.Handle(@event, CancellationToken.None);

            var loggedMessage = loggerBuilder.GetLoggedMessage();
            loggedMessage.Should().NotBeNull("uma mensagem de log deve ter sido capturada");

            loggedMessage.Should().Contain(library.Id.ToString());
            loggedMessage.Should().Contain(library.UserId.ToString());
        }
    }
}
