using FCG.Application.UseCases.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Models;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FCG.FunctionalTests.Fixtures.Authentication
{
    public class LoginFixture
    {
        public LoginFixture()
        {
            var userRepository = ReadOnlyUserRepositoryBuilder.Build();
            var refreshToken = RefreshTokenBuilder.Build();
            Setup(refreshToken);
            var tokenService = TokenServiceBuilder.Build();
            var jwtSettings = Options.Create(JwtSettingsBuilder.Build());
            var passwordEcrypter = PasswordEncrypterServiceBuilder.Build();
            var logger = new Mock<ILogger<LoginUseCase>>().Object;
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            LoginUseCase = new LoginUseCase(userRepository, tokenService, jwtSettings, passwordEcrypter, logger, correlationIdProvider);
            LoginInput = LoginInputBuilder.Build();
        }

        public LoginUseCase LoginUseCase { get; }
        public LoginInput LoginInput { get; }

        private static void Setup(RefreshToken refreshToken)
        {
            var user = UserBuilder.Build();
            ReadOnlyUserRepositoryBuilder.SetupGetByEmailAsync(user);
            TokenServiceBuilder.SetupGenerateAccessToken("access_token");
            TokenServiceBuilder.SetupGenerateRefreshToken("refresh_token");
            TokenServiceBuilder.SetupSaveRefreshTokenAsync(refreshToken);
            PasswordEncrypterServiceBuilder.SetupIsValid(true);
        }
    }
}