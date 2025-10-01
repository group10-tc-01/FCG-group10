using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Events
{
    public class LoggerMockBuilder<THandler>
    {
        private readonly Mock<ILogger<THandler>> _loggerMock = new();

        private string? _loggedMessage;

        public Mock<ILogger<THandler>> Build() => _loggerMock;

        public string? GetLoggedMessage() => _loggedMessage;

        public LoggerMockBuilder<THandler> CaptureInformationLog()
        {
            _loggerMock
                .Setup(x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ))
                .Callback(new InvocationAction(invocation =>
                {
                    var state = invocation.Arguments[2];
                    _loggedMessage = state?.ToString();
                }))
                .Verifiable();

            return this;
        }

        public void VerifyInformationLogContains(params string[] expectedParts)
        {
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) =>
                        expectedParts.All(p => v.ToString()!.Contains(p))),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}
