using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.PromotionRepository
{
    public interface IWriteOnlyPromotionRepository
    {
        Task AddAsync(Promotion promotion, CancellationToken cancellationToken = default);
        Task UpdateAsync(Promotion promotion, CancellationToken cancellationToken = default);
        Task DeleteAsync(Promotion promotion, CancellationToken cancellationToken = default);
    }
}
