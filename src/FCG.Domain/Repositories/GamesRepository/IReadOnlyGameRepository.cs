using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.GamesRepository
{
    public interface IReadOnlyGameRepository
    {
        IQueryable<Game?> GetAllAsQueryable();
        Task<Game?> GetByNameAsync(string name);
    }
}
