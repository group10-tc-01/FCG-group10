using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.GamesRepository
{
    public interface IReadOnlyGameRepository
    {
        Task<Game?> GetByNameAsync(string name);
    }
}
