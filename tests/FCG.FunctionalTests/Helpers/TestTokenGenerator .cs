using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System;

namespace FCG.FunctionalTests.Helpers
{
    public static class TestTokenGenerator
    {
        public static string GenerateToken(IConfiguration configuration, Guid userId, string role)
        {
            var jwtSettingsSection = configuration.GetSection("JwtSettings");

            // Leitura das configurações
            var secretKey = jwtSettingsSection["SecretKey"];
            var issuer = jwtSettingsSection["Issuer"];
            var audience = jwtSettingsSection["Audience"];

            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("As configurações do JWT (SecretKey, Issuer, Audience) são obrigatórias para gerar tokens de teste.");
            }

            // Lê o tempo de expiração do appsettings.json
            var expirationMinutes = configuration.GetValue<int>("JwtSettings:AccessTokenExpirationMinutes");


            var tokenHandler = new JwtSecurityTokenHandler();

            // Usamos UTF8 para robustez e compatibilidade com o middleware
            var key = Encoding.UTF8.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // ⬅️ USA O VALOR LIDO DO APPSETTINGS
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