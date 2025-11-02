using FCG.Application.UseCases.Authentication.RefreshToken;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.RefreshToken;
using FCG.CommomTestsUtilities.Builders.Models;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Messages;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FCG.UnitTests.Application.UseCases.Authentication.Refresh
{
    public class RefreshTokenUseCaseTest
    {
        private readonly ITokenService _tokenService;
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IRefreshTokenUseCase _sut;

        public RefreshTokenUseCaseTest()
        {
            _tokenService = TokenServiceBuilder.Build();
            _readOnlyUserRepository = ReadOnlyUserRepositoryBuilder.Build();
            var logger = new Mock<ILogger<RefreshTokenUseCase>>().Object;
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            _sut = new RefreshTokenUseCase(_tokenService, _readOnlyUserRepository, Options.Create(JwtSettingsBuilder.Build()), logger, correlationIdProvider);
        }

        [Fact]
        public async Task Given_ValidRefreshToken_When_Handle_Then_ShouldReturnNewTokens()
        {
            // Arrange
            var input = RefreshTokenInputBuilder.Build();
            var user = UserBuilder.Build();
            var expiresInDays = 1;
            var newRefreshToken = RefreshTokenBuilder.Build();
            Setup(user, newRefreshToken);

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("new_access_token");
            result.RefreshToken.Should().Be(newRefreshToken.Token);
            result.ExpiresInDays.Should().Be(expiresInDays);
        }

        [Fact]
        public async Task Given_InvalidRefreshToken_When_Handle_Then_ShouldThrowUnauthorizedException()
        {
            // Arrange
            var input = RefreshTokenInputBuilder.Build();
            TokenServiceBuilder.SetupValidateRefreshTokenAsync(null);

            // Act & Assert
            var act = async () => await _sut.Handle(input, CancellationToken.None);
            await act.Should().ThrowAsync<UnauthorizedException>().WithMessage(ResourceMessages.InvalidRefreshToken);
        }

        [Fact]
        public async Task Given_ValidTokenButUserNotFound_When_Handle_Then_ShouldThrowUnauthorizedException()
        {
            // Arrange
            var input = RefreshTokenInputBuilder.Build();
            var userId = Guid.NewGuid().ToString();
            TokenServiceBuilder.SetupValidateRefreshTokenAsync(userId);
            ReadOnlyUserRepositoryBuilder.SetupGetByIdAsync(null);

            // Act & Assert
            var act = async () => await _sut.Handle(input, CancellationToken.None);
            await act.Should().ThrowAsync<UnauthorizedException>().WithMessage(ResourceMessages.InvalidRefreshToken);
        }

        [Fact]
        public async Task Given_ValidRefreshToken_When_Handle_Then_ShouldRevokeOldToken()
        {
            // Arrange
            var input = RefreshTokenInputBuilder.BuildWithToken("old-refresh-token");
            var user = UserBuilder.Build();
            var newRefreshToken = RefreshTokenBuilder.Build();
            Setup(user, newRefreshToken);

            // Act
            await _sut.Handle(input, CancellationToken.None);

            // Assert
            TokenServiceBuilder.VerifyRevokeRefreshTokenAsyncWasCalledWith("old-refresh-token");
        }

        [Fact]
        public async Task Given_ValidRefreshToken_When_Handle_Then_ShouldSaveNewRefreshToken()
        {
            // Arrange
            var input = RefreshTokenInputBuilder.Build();
            var user = UserBuilder.Build();
            var newRefreshToken = RefreshTokenBuilder.Build();
            Setup(user, newRefreshToken);

            // Act
            await _sut.Handle(input, CancellationToken.None);

            // Assert
            TokenServiceBuilder.VerifySaveRefreshTokenAsyncWasCalled();
        }

        private static void Setup(User user, RefreshToken newRefreshToken)
        {
            TokenServiceBuilder.SetupValidateRefreshTokenAsync(user.Id.ToString());
            ReadOnlyUserRepositoryBuilder.SetupGetByIdAsync(user);
            TokenServiceBuilder.SetupRevokeRefreshTokenAsync();
            TokenServiceBuilder.SetupGenerateAccessToken("new_access_token");
            TokenServiceBuilder.SetupGenerateRefreshToken("new_refresh_token");
            TokenServiceBuilder.SetupSaveRefreshTokenAsync(newRefreshToken);
        }
    }
}