using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.RefreshTokenRepository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId);
        Task UpdateAsync(RefreshToken refreshToken);
        Task RevokeAllByUserIdAsync(Guid userId);
    }
}