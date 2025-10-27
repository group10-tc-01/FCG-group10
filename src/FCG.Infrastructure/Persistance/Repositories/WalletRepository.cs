using FCG.Domain.Entities;
using FCG.Domain.Repositories.WalletRepository;

namespace FCG.Infrastructure.Persistance.Repositories
{
    public class WalletRepository : IWriteOnlyWalletRepository
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
    }
}
