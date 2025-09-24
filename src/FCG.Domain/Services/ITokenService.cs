using FCG.Domain.Entities;

namespace FCG.Domain.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        Task<string?> ValidateRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task<RefreshToken> SaveRefreshTokenAsync(string token, Guid userId);
    }
}