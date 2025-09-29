using FCG.Application.UseCases.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Models;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using Microsoft.Extensions.Options;

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

            LoginUseCase = new LoginUseCase(userRepository, tokenService, jwtSettings);
            LoginInput = LoginInputBuilder.Build();
        }

        public LoginUseCase LoginUseCase { get; }
        public LoginInput LoginInput { get; }

        private static void Setup(RefreshToken refreshToken)
        {
            var user = UserBuilder.Build();
            ReadOnlyUserRepositoryBuilder.SetupGetByEmailAndPasswordAsync(user);
            TokenServiceBuilder.SetupGenerateAccessToken("access_token");
            TokenServiceBuilder.SetupGenerateRefreshToken("refresh_token");
            TokenServiceBuilder.SetupSaveRefreshTokenAsync(refreshToken);
        }
    }
}