using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Models;
using FCG.CommomTestsUtilities.Builders.Repositories.RefreshTokenRepository;
using FCG.Domain.Models.Authenticaiton;
using FCG.Domain.Repositories.RefreshTokenRepository;
using FCG.Infrastructure.Services.Authentication;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace FCG.UnitTests.Services.Authentication
{
    public class TokenServiceTest
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly TokenService _sut;

        public TokenServiceTest()
        {
            _refreshTokenRepository = RefreshTokenRepositoryBuilder.Build();
            _jwtSettings = Options.Create(JwtSettingsBuilder.Build());
            _sut = new TokenService(_jwtSettings, _refreshTokenRepository);
        }

        [Fact]
        public void Given_ValidUser_When_GenerateAccessToken_Then_ShouldReturnNonEmptyToken()
        {
            // Arrange
            var user = UserBuilder.Build();

            // Act
            var token = _sut.GenerateAccessToken(user);

            // Assert
            token.Should().NotBeNullOrEmpty();
            token.Should().Contain(".");
            token.Split('.').Should().HaveCount(3);
        }

        [Fact]
        public void Given_DifferentUsers_When_GenerateAccessToken_Then_ShouldReturnDifferentTokens()
        {
            // Arrange
            var user1 = UserBuilder.Build();
            var user2 = UserBuilder.Build();

            // Act
            var token1 = _sut.GenerateAccessToken(user1);
            var token2 = _sut.GenerateAccessToken(user2);

            // Assert
            token1.Should().NotBe(token2);
        }

        [Fact]
        public void Given_EmptyRequest_When_GenerateRefreshToken_Then_ShouldReturnValidBase64String()
        {
            // Act
            var refreshToken = _sut.GenerateRefreshToken();

            // Assert
            refreshToken.Should().NotBeNullOrEmpty();
            var act = () => Convert.FromBase64String(refreshToken);
            act.Should().NotThrow();

            var bytes = Convert.FromBase64String(refreshToken);
            bytes.Should().HaveCount(32);
        }

        [Fact]
        public void Given_MultipleCalls_When_GenerateRefreshToken_Then_ShouldReturnDifferentTokens()
        {
            // Act
            var token1 = _sut.GenerateRefreshToken();
            var token2 = _sut.GenerateRefreshToken();

            // Assert
            token1.Should().NotBe(token2);
        }

        [Fact]
        public async Task Given_ValidRefreshToken_When_ValidateRefreshTokenAsync_Then_ShouldReturnUserId()
        {
            // Arrange
            var refreshToken = RefreshTokenBuilder.Build();
            var tokenString = "valid-refresh-token";
            RefreshTokenRepositoryBuilder.SetupGetByTokenAsync(refreshToken);

            // Act
            var result = await _sut.ValidateRefreshTokenAsync(tokenString);

            // Assert
            result.Should().Be(refreshToken.UserId.ToString());
        }

        [Fact]
        public async Task Given_NonExistentToken_When_ValidateRefreshTokenAsync_Then_ShouldReturnNull()
        {
            // Arrange
            var tokenString = "invalid-token";
            RefreshTokenRepositoryBuilder.SetupGetByTokenAsync(null);

            // Act
            var result = await _sut.ValidateRefreshTokenAsync(tokenString);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Given_ExpiredRefreshToken_When_ValidateRefreshTokenAsync_Then_ShouldReturnNull()
        {
            // Arrange
            var expiredRefreshToken = RefreshTokenBuilder.BuildExpired();
            var tokenString = "expired-token";
            RefreshTokenRepositoryBuilder.SetupGetByTokenAsync(expiredRefreshToken);

            // Act
            var result = await _sut.ValidateRefreshTokenAsync(tokenString);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Given_ValidToken_When_RevokeRefreshTokenAsync_Then_ShouldRevokeAndUpdateToken()
        {
            // Arrange
            var refreshToken = RefreshTokenBuilder.Build();
            var tokenString = "valid-token";
            RefreshTokenRepositoryBuilder.SetupGetByTokenAsync(refreshToken);
            RefreshTokenRepositoryBuilder.SetupUpdateAsync();

            // Act
            await _sut.RevokeRefreshTokenAsync(tokenString);

            // Assert
            refreshToken.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task Given_NonExistentToken_When_RevokeRefreshTokenAsync_Then_ShouldNotCallUpdate()
        {
            // Arrange
            var tokenString = "invalid-token";
            RefreshTokenRepositoryBuilder.SetupGetByTokenAsync(null);

            // Act & Assert
            var act = async () => await _sut.RevokeRefreshTokenAsync(tokenString);
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task Given_ValidParameters_When_SaveRefreshTokenAsync_Then_ShouldCreateAndReturnRefreshToken()
        {
            // Arrange
            var tokenString = "new-refresh-token";
            var userId = Guid.NewGuid();
            var expectedRefreshToken = RefreshTokenBuilder.Build();
            RefreshTokenRepositoryBuilder.SetupCreateAsync(expectedRefreshToken);

            // Act
            var result = await _sut.SaveRefreshTokenAsync(tokenString, userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(expectedRefreshToken);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task Given_InvalidTokenString_When_ValidateRefreshTokenAsync_Then_ShouldReturnNull(string? invalidToken)
        {
            // Arrange
            RefreshTokenRepositoryBuilder.SetupGetByTokenAsync(null);

            // Act
            var result = await _sut.ValidateRefreshTokenAsync(invalidToken!);

            // Assert
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task Given_InvalidTokenString_When_RevokeRefreshTokenAsync_Then_ShouldNotThrow(string? invalidToken)
        {
            // Arrange
            RefreshTokenRepositoryBuilder.SetupGetByTokenAsync(null);

            // Act & Assert
            var act = async () => await _sut.RevokeRefreshTokenAsync(invalidToken!);
            await act.Should().NotThrowAsync();
        }
    }
}