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
    public class LoginUseCaseTest
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordEncrypter _passwordEncrypter;
        private readonly ILoginUseCase _sut;

        public LoginUseCaseTest()
        {
            _readOnlyUserRepository = ReadOnlyUserRepositoryBuilder.Build();
            _tokenService = TokenServiceBuilder.Build();
            _passwordEncrypter = PasswordEncrypterServiceBuilder.Build();
            _sut = new LoginUseCase(_readOnlyUserRepository, _tokenService, Options.Create(JwtSettingsBuilder.Build()), _passwordEncrypter);
        }

        [Fact]
        public async Task Given_ValidLogin_When_Handle_Then_ShouldReturnOutput()
        {
            // Arrange
            var input = LoginInputBuilder.Build();
            var refreshToken = RefreshTokenBuilder.Build();
            Setup(refreshToken);

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("access_token");
            result.RefreshToken.Should().Be(refreshToken.Token);
            result.ExpiresInMinutes.Should().Be(1);
        }

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
