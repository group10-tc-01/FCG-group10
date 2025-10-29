using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.WalletRepository
{
    public interface IWriteOnlyWalletRepository
    {
        Task AddAsync(Wallet wallet);
    }
}
