using FCG.Domain.Entities;
using FCG.Domain.Repositories.UserRepository;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Repositories.UserRepository
{
    public static class ReadOnlyUserRepositoryBuilder
    {
        private static readonly Mock<IReadOnlyUserRepository> _mock = new Mock<IReadOnlyUserRepository>();

        public static IReadOnlyUserRepository Build() => _mock.Object;

        public static void SetupGetByEmailAsync(User? user)
        {
            _mock.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        }

        public static void SetupGetByIdAsync(User? user)
        {
            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(user);
        }
    }
}
