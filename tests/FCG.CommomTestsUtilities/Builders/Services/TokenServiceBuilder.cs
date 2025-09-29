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

        public static void VerifyRevokeRefreshTokenAsyncWasCalledWith(string refreshToken)
        {
            _mock.Verify(service => service.RevokeRefreshTokenAsync(refreshToken), Times.Once);
        }

        public static void VerifySaveRefreshTokenAsyncWasCalled()
        {
            _mock.Verify(service => service.SaveRefreshTokenAsync(It.IsAny<string>(), It.IsAny<Guid>()), Times.AtLeastOnce);
        }

        public static void VerifyGenerateAccessTokenWasCalled()
        {
            _mock.Verify(service => service.GenerateAccessToken(It.IsAny<User>()), Times.Once);
        }

        public static void VerifyGenerateRefreshTokenWasCalled()
        {
            _mock.Verify(service => service.GenerateRefreshToken(), Times.Once);
        }

        public static void VerifyValidateRefreshTokenAsyncWasCalled()
        {
            _mock.Verify(service => service.ValidateRefreshTokenAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
