using FCG.Domain.Models.Authenticaiton;

namespace FCG.CommomTestsUtilities.Builders.Models
{
    public static class JwtSettingsBuilder
    {
        public static JwtSettings Build()
        {
            return new JwtSettings
            {
                SecretKey = "teste-key-teste-key-teste-key-teste-key-teste-key-teste-key-teste-key-teste-key",
                Issuer = "FCG-API-TEST",
                Audience = "FCG-Client-TEST",
                AccessTokenExpirationMinutes = 1,
                RefreshTokenExpirationDays = 1
            };
        }

        public static JwtSettings BuildWithShortExpiration()
        {
            return new JwtSettings
            {
                SecretKey = "teste-key-teste-key-teste-key-teste-key-teste-key-teste-key-teste-key-teste-key",
                Issuer = "FCG-API-TEST",
                Audience = "FCG-Client-TEST",
                AccessTokenExpirationMinutes = 0,
                RefreshTokenExpirationDays = 1
            };
        }
    }
}
