using FCG.Domain.Entities;
using FCG.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public static void SetupValidateRefreshTokenAsyncWithInvalidToken(string? userId)
        {
            _mock.Setup(service => service.ValidateRefreshTokenAsync("invalid_refresh_token")).ReturnsAsync(userId);
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

        public static string GenerateToken(IConfiguration configuration, Guid userId, string role)
        {
            var jwtSettingsSection = configuration.GetSection("JwtSettings");


            var secretKey = jwtSettingsSection["SecretKey"];
            var issuer = jwtSettingsSection["Issuer"];
            var audience = jwtSettingsSection["Audience"];

            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("As configurações do JWT (SecretKey, Issuer, Audience) são obrigatórias para gerar tokens de teste.");
            }

            var expirationMinutes = configuration.GetValue<int>("JwtSettings:AccessTokenExpirationMinutes");

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = issuer,
                Audience = audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
