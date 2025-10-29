using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.GamesRepository
{
    public interface IReadOnlyGameRepository
    {
        IQueryable<Game?> GetAllAsQueryable();
        Task<Game?> GetByNameAsync(string name);
        Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
