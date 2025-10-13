using FCG.Application.UseCases.AdminUsers.GetAllUsers;
using FCG.Domain.Entities;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.ValueObjects;
using Moq;

namespace FCG.Tests.Application.UseCases.Users
{
    public class GetAllUsersUseCaseTests
    {
        private readonly Mock<IReadOnlyUserRepository> _userRepositoryMock;
        private readonly GetAllUsersUseCase _handler;

        public GetAllUsersUseCaseTests()
        {
            _userRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _handler = new GetAllUsersUseCase(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Given_EmailFilter_When_HandlingQuery_Then_ShouldReturnFilteredPagedListido()
        {
            // ARRANGE
            var users = new List<User>
            {
            };

            var query = new GetAllUserCaseQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Email = "test.com"
            };

            var expectedUsers = users.Where(u => u.Email.Value.Contains("test.com")).ToList();
            var expectedTotalCount = expectedUsers.Count;

            var expectedResultTuple = (
                Items: (IEnumerable<User>)expectedUsers,
                TotalCount: expectedTotalCount
            );

            _userRepositoryMock
                .Setup(repo => repo.GetQueryableAllUsers(
                    It.Is<string?>(match => match == query.Email),

                    It.Is<string?>(match => match == query.Role),

                    It.Is<int>(match => match == query.PageNumber),

                    It.Is<int>(match => match == query.PageSize),

                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResultTuple);

            // ACT
            var result = await _handler.Handle(query, CancellationToken.None);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(expectedTotalCount, result.TotalCount);
        }

        [Fact]
        public async Task Given_RoleFilter_When_HandlingQuery_Then_ShouldReturnOnlyMatchingRoleUsers()
        {
            // ARRANGE
            var AdminRole = FCG.Domain.Enum.Role.Admin;
            var CommonRole = FCG.Domain.Enum.Role.User;

            var adminUser = User.Create(
                "Admin A",
                "admin@fcg.com",
                "senha123!D",
                AdminRole
            );

            var commonUser = User.Create(
                "Common B",
                "common@fcg.com",
                "senha123!@F",
                CommonRole
            );

            var expectedUsers = new List<User> { adminUser };
            var expectedTotalCount = 1;

            var query = new GetAllUserCaseQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Role = AdminRole.ToString()
            };

            var expectedResultTuple = (
                Items: (IEnumerable<User>)expectedUsers,
                TotalCount: expectedTotalCount
            );

            _userRepositoryMock
                .Setup(repo => repo.GetQueryableAllUsers(
                    It.IsAny<string>(), // Filtro de Email
                    It.Is<string>(r => r == query.Role),
                    It.IsAny<int>(), // PageNumber
                    It.IsAny<int>(), // PageSize
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResultTuple);

            // ACT
            var result = await _handler.Handle(query, CancellationToken.None);

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(expectedTotalCount, result.TotalCount);

        }
    }
}
