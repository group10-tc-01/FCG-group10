using FCG.Domain.Entities;
using FCG.Domain.Services;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Services
{
    public static class TokenServiceBuilder
    {
        private static readonly Mock<ITokenService> _mock = new Mock<ITokenService>();

        public static ITokenService Build() => _mock.Object;

        public static void SetupGenerateAccessToken(string token)
        {
            _mock.Setup(service => service.GenerateAccessToken(It.IsAny<User>())).Returns(token);
        }

        public static void SetupGenerateRefreshToken(string token)
        {
            _mock.Setup(service => service.GenerateRefreshToken()).Returns(token);
        }

        public static void SetupValidateRefreshTokenAsync(string? userId)
        {
            _mock.Setup(service => service.ValidateRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync(userId);
        }

        public static void SetupRevokeRefreshTokenAsync()
        {
            _mock.Setup(service => service.RevokeRefreshTokenAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
        }

        public static void SetupSaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            _mock.Setup(service => service.SaveRefreshTokenAsync(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(refreshToken);
        }
    }
}
