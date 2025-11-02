using FCG.Application.UseCases.Authentication.RefreshToken;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.RefreshToken;
using FCG.CommomTestsUtilities.Builders.Models;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using Microsoft.Extensions.Options;

namespace FCG.FunctionalTests.Fixtures.Authentication
{
    public class RefreshTokenFixture
    {
        public RefreshTokenFixture()
        {
            var tokenService = TokenServiceBuilder.Build();
            var userRepository = ReadOnlyUserRepositoryBuilder.Build();
            var jwtSettings = Options.Create(JwtSettingsBuilder.Build());
            var refreshToken = RefreshTokenBuilder.Build();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<RefreshTokenUseCase>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            Setup(refreshToken);

            RefreshTokenUseCase = new RefreshTokenUseCase(tokenService, userRepository, jwtSettings, logger, correlationIdProvider);
            RefreshTokenInput = RefreshTokenInputBuilder.Build();
            RefreshTokenInputWithValidToken = RefreshTokenInputBuilder.BuildWithToken("valid_refresh_token");
            RefreshTokenInputWithInvalidToken = RefreshTokenInputBuilder.BuildWithToken("invalid_refresh_token");
            RefreshTokenInputWithEmptyToken = RefreshTokenInputBuilder.BuildWithEmptyToken();
        }

        public RefreshTokenUseCase RefreshTokenUseCase { get; }
        public RefreshTokenInput RefreshTokenInput { get; }
        public RefreshTokenInput RefreshTokenInputWithValidToken { get; }
        public RefreshTokenInput RefreshTokenInputWithInvalidToken { get; }
        public RefreshTokenInput RefreshTokenInputWithEmptyToken { get; }

        private static void Setup(RefreshToken refreshToken)
        {
            var user = UserBuilder.Build();
            ReadOnlyUserRepositoryBuilder.SetupGetByIdAsync(user);
            TokenServiceBuilder.SetupValidateRefreshTokenAsync(user.Id.ToString());
            TokenServiceBuilder.SetupValidateRefreshTokenAsyncWithInvalidToken(null);
            TokenServiceBuilder.SetupRevokeRefreshTokenAsync();
            TokenServiceBuilder.SetupGenerateAccessToken("new_access_token");
            TokenServiceBuilder.SetupGenerateRefreshToken("new_refresh_token");
            TokenServiceBuilder.SetupSaveRefreshTokenAsync(refreshToken);
        }
    }
}