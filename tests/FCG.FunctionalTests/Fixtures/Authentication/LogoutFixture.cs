using FCG.Application.UseCases.Authentication.Logout;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Logout;
using FCG.CommomTestsUtilities.Builders.Repositories.RefreshTokenRepository;

namespace FCG.FunctionalTests.Fixtures.Authentication
{
    public class LogoutFixture
    {
        public LogoutFixture()
        {
            var refreshTokenRepository = RefreshTokenRepositoryBuilder.Build();
            Setup();

            LogoutUseCase = new LogoutUseCase(refreshTokenRepository);
            LogoutInput = LogoutInputBuilder.Build();
            LogoutInputWithUserId = LogoutInputBuilder.BuildWithUserId(Guid.NewGuid());
            LogoutInputWithEmptyUserId = LogoutInputBuilder.BuildWithEmptyUserId();
        }

        public LogoutUseCase LogoutUseCase { get; }
        public LogoutInput LogoutInput { get; }
        public LogoutInput LogoutInputWithUserId { get; }
        public LogoutInput LogoutInputWithEmptyUserId { get; }

        private static void Setup()
        {
            RefreshTokenRepositoryBuilder.SetupRevokeAllByUserIdAsync();
        }
    }
}
