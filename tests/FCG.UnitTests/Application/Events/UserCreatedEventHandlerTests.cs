using FCG.Application.EventsHandlers.Users;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Events;
using FCG.Domain.Events.User;
using FluentAssertions;

namespace FCG.UnitTests.Application.Events
{
    public class UserCreatedEventHandlerTests
    {
        [Fact]
        public async Task Handler_RegistersLog_WhenEventIsPublished()
        {
            var loggerBuilder = new LoggerMockBuilder<UserCreatedEventHandler>().CaptureInformationLog();
            var handler = new UserCreatedEventHandler(loggerBuilder.Build().Object);

            var user = UserBuilder.Build();

            var @event = new UserCreatedEvent(user.Id, user.Name, user.Email);

            await handler.Handle(@event, CancellationToken.None);

            var loggedMessage = loggerBuilder.GetLoggedMessage();
            loggedMessage.Should().NotBeNull("uma mensagem de log deve ter sido capturada");

            loggedMessage.Should().Contain(user.Id.ToString());
            loggedMessage.Should().Contain(user.Name);
            loggedMessage.Should().Contain(user.Email);
        }
    }
}
