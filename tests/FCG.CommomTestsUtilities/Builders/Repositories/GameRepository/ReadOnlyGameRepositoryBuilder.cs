using FCG.Domain.Entities;
using FCG.Domain.Repositories.GamesRepository;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Repositories.GameRepository
{
    public static class ReadOnlyGameRepositoryBuilder
    {
        private static readonly Mock<IReadOnlyGameRepository> _mock = new Mock<IReadOnlyGameRepository>();

        public static IReadOnlyGameRepository Build() => _mock.Object;

        public static void SetupGetByNameAsync(Game? game)
        {
            _mock.Setup(repo => repo.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(game);
        }

        public static void SetupGetByNameAsyncWithSpecificName(string name, Game? game)
        {
            _mock.Setup(repo => repo.GetByNameAsync(name)).ReturnsAsync(game);
        }

        public static void VerifyGetByNameAsyncWasCalled()
        {
            _mock.Verify(repo => repo.GetByNameAsync(It.IsAny<string>()), Times.AtLeastOnce);
        }

        public static void VerifyGetByNameAsyncWasCalledWith(string name)
        {
            _mock.Verify(repo => repo.GetByNameAsync(name), Times.AtLeastOnce);
        }

        public static void SetupGetAllAsQueryable(IQueryable<Game> query)
        {
            _mock.Setup(repo => repo.GetAllAsQueryable())
                .Returns(query);
        }

        public static void VerifyGetAllAsQueryableWasCalled()
        {
            _mock.Verify(repo => repo.GetAllAsQueryable(), Times.AtLeastOnce);
        }

        public static void Reset()
        {
            _mock.Reset();
        }
    }
}