using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FCG.FunctionalTests.Helpers
{
    public static class TestTokenGenerator
    {
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