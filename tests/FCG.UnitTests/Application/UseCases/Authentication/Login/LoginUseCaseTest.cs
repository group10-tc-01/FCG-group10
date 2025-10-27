using FCG.Application.UseCases.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Models;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace FCG.UnitTests.Application.UseCases.Authentication.Login
{
    public class LoginUseCaseTest : IDisposable
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly ILoginUseCase _sut;

        public LoginUseCaseTest()
        {
            ResetAllBuilders();

            _readOnlyUserRepository = ReadOnlyUserRepositoryBuilder.Build();
            _tokenService = TokenServiceBuilder.Build();
            _passwordEncrypter = PasswordEncrypterServiceBuilder.Build();
            _sut = new LoginUseCase(
                _readOnlyUserRepository,
                _tokenService,
                Options.Create(JwtSettingsBuilder.Build()),
                _passwordEncrypter
            );
        }

        public void Dispose()
        {
            ResetAllBuilders();
        }

        [Fact]
        public async Task Given_ValidLogin_When_Handle_Then_ShouldReturnOutput()
        {
            // Arrange
            var input = LoginInputBuilder.Build();
            var user = UserBuilder.Build();

            var expectedAccessToken = "new_access_token";
            var expectedRefreshToken = "new_refresh_token";
            var refreshToken = RefreshToken.Create(expectedRefreshToken, user.Id, TimeSpan.FromDays(7));

            // Setup dos mocks
            ReadOnlyUserRepositoryBuilder.SetupGetByEmailAsync(user);
            PasswordEncrypterServiceBuilder.SetupIsValid(true);
            TokenServiceBuilder.SetupGenerateAccessToken(expectedAccessToken);
            TokenServiceBuilder.SetupGenerateRefreshToken(expectedRefreshToken);
            TokenServiceBuilder.SetupSaveRefreshTokenAsync(refreshToken);

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be(expectedAccessToken);
            result.RefreshToken.Should().Be(expectedRefreshToken);
            result.ExpiresInMinutes.Should().Be(1);

        }

        private static void ResetAllBuilders()
        {
        }
    }
}