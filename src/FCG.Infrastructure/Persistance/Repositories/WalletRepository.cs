using FCG.Domain.Entities;
using FCG.Domain.Repositories.WalletRepository;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Persistance.Repositories
{
    public class WalletRepository : IWriteOnlyWalletRepository, IReadOnlyWalletRepository
    {
        private readonly FcgDbContext _fcgDbContext;
        public WalletRepository(FcgDbContext context)
        {
            _fcgDbContext = context;
        }

        public async Task AddAsync(Wallet wallet)
        {
            await _fcgDbContext.Wallets.AddAsync(wallet);
        }

        public async Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _fcgDbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == userId, cancellationToken);
        }

        public async Task<Wallet?> GetByIdAsync(Guid walletId, CancellationToken cancellationToken)
        {
            return await _fcgDbContext.Wallets.FirstOrDefaultAsync(w => w.Id == walletId, cancellationToken);
        }
    }
}
