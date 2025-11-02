using FCG.Domain.Entities;
using FCG.Domain.Repositories.GamesRepository;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Repositories.GameRepository
{
    public static class WriteOnlyGameRepositoryBuilder
    {
        private static readonly Mock<IWriteOnlyGameRepository> _mock = new Mock<IWriteOnlyGameRepository>();

        public static IWriteOnlyGameRepository Build() => _mock.Object;

        public static void SetupAddAsync()
        {
            _mock.Setup(repo => repo.AddAsync(It.IsAny<Game>())).Returns(Task.CompletedTask);
        }

        public static void VerifyAddAsyncWasCalled()
        {
            _mock.Verify(repo => repo.AddAsync(It.IsAny<Game>()), Times.AtLeastOnce);
        }

        public static void VerifyAddAsyncWasCalledWith(Game game)
        {
            _mock.Verify(repo => repo.AddAsync(game), Times.AtLeastOnce);
        }
    }
}