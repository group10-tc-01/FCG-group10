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
            if (user != null)
            {
                _mock.Setup(repo => repo.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            }
            else
            {
                _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);
            }
        }

        public static void SetupGetAllUsersAsync(List<User> users)
        {
            _mock.Setup(repo => repo.GetAllUsersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((users, users.Count));
        }

        public static void SetupGetAllUsersWithFilters(IQueryable<User> queryable)
        {
            _mock.Setup(repo => repo.GetAllUsersWithFilters(
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns(queryable);
        }
    }
}
