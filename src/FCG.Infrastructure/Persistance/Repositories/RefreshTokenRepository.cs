using FCG.Domain.Entities;
using FCG.Domain.Repositories.RefreshTokenRepository;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly FcgDbContext _fcgDbContext;

        public RefreshTokenRepository(FcgDbContext fcgDbContext) => _fcgDbContext = fcgDbContext;

        public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
        {
            await _fcgDbContext.RefreshTokens.AddAsync(refreshToken);
            await _fcgDbContext.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _fcgDbContext.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId)
        {
            return await _fcgDbContext.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            _fcgDbContext.RefreshTokens.Update(refreshToken);

            await _fcgDbContext.SaveChangesAsync();
        }

        public async Task RevokeAllByUserIdAsync(Guid userId)
        {
            var tokens = await GetByUserIdAsync(userId);

            foreach (var token in tokens.Where(t => t.IsActive))
            {
                token.Revoke("Revoked all tokens");
            }

            await _fcgDbContext.SaveChangesAsync();
        }
    }
}