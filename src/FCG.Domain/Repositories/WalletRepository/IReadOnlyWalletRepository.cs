using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.WalletRepository
{
    public interface IReadOnlyWalletRepository
    {
        Task<Wallet?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
