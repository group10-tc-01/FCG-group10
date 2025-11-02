using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.WalletRepository
{
    public interface IReadOnlyWalletRepository
    {
        Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Wallet?> GetByIdAsync(Guid walletId, CancellationToken cancellationToken);
    }
}
