using FCG.Domain.Entities;
using FCG.Domain.Repositories.UserRepository;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Repositories.UserRepository
{
    public static class WriteOnlyUserRepositoryBuilder
    {
        private static readonly Mock<IWriteOnlyUserRepository> _mock = new Mock<IWriteOnlyUserRepository>();

        public static IWriteOnlyUserRepository Build() => _mock.Object;

        public static void SetupAddAsync()
        {
            _mock.Setup(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<Wallet>()))
                .Returns(Task.CompletedTask);
        }

        public static void SetupUpdateAsync()
        {
            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        public static void Reset()
        {
            _mock.Reset();
        }
    }
}