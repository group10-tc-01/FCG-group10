using FCG.Domain.Services;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Services
{
    public static class CorrelationIdProviderBuilder
    {
        private static readonly Mock<ICorrelationIdProvider> _mock = new Mock<ICorrelationIdProvider>();

        public static ICorrelationIdProvider Build() => _mock.Object;

        public static void SetupGetCorrelationId(string correlationId)
        {
            _mock.Setup(service => service.GetCorrelationId()).Returns(correlationId);
        }

        public static void Reset()
        {
            _mock.Reset();
        }
    }
}
