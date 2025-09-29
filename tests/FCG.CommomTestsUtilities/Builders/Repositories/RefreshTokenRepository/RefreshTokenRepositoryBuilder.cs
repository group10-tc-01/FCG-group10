using FCG.Domain.Entities;
using FCG.Domain.Repositories.RefreshTokenRepository;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Repositories.RefreshTokenRepository
{
    public static class RefreshTokenRepositoryBuilder
    {
        private static readonly Mock<IRefreshTokenRepository> _mock = new Mock<IRefreshTokenRepository>();

        public static IRefreshTokenRepository Build() => _mock.Object;

        public static void SetupCreateAsync(RefreshToken refreshToken)
        {
            _mock.Setup(repo => repo.CreateAsync(It.IsAny<RefreshToken>())).ReturnsAsync(refreshToken);
        }

        public static void SetupGetByTokenAsync(RefreshToken? refreshToken)
        {
            _mock.Setup(repo => repo.GetByTokenAsync(It.IsAny<string>())).ReturnsAsync(refreshToken);
        }

        public static void SetupGetByUserIdAsync(IEnumerable<RefreshToken> refreshTokens)
        {
            _mock.Setup(repo => repo.GetByUserIdAsync(It.IsAny<Guid>())).ReturnsAsync(refreshTokens);
        }

        public static void SetupUpdateAsync()
        {
            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<RefreshToken>())).Returns(Task.CompletedTask);
        }

        public static void SetupRevokeAllByUserIdAsync()
        {
            _mock.Setup(repo => repo.RevokeAllByUserIdAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);
        }

        public static void VerifyRevokeAllByUserIdAsyncWasCalledWith(Guid userId)
        {
            _mock.Verify(repo => repo.RevokeAllByUserIdAsync(userId), Times.Once);
        }
    }
}