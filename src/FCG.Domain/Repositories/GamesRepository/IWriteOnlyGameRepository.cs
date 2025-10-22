using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.GamesRepository
{
    public interface IWriteOnlyGameRepository
    {
        Task AddAsync(Game game);
    }
}
