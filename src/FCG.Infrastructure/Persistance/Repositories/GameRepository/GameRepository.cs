using FCG.Domain.Entities;
using FCG.Domain.Repositories.GamesRepository;

namespace FCG.Infrastructure.Persistance.Repositories.GameRepository
{
    public class GameRepository : IWriteOnlyGameRepository
    {
        private readonly FcgDbContext _fcgDbContext;

        public GameRepository(FcgDbContext context)
        {
            _fcgDbContext = context;
        }

        public Task AddAsync(Game game)
        {
            _fcgDbContext.Games.Add(game);
            return Task.CompletedTask;
        }
    }
}
