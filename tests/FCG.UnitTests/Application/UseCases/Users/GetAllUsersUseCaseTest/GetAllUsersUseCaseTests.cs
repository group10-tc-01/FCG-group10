using FCG.Application.UseCases.AdminUsers.GetAllUsers;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Repositories.UserRepository;
using FluentAssertions;
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
        public async Task Given_NoFilters_When_HandlingQuery_Then_ShouldReturnAllUsersPagedList()
        {
            // Given
            var users = new List<User>
            {
                User.Create("User A", "usera@test.com", "Password@123", Role.User),
                User.Create("User B", "userb@test.com", "Password@123", Role.Admin),
                User.Create("User C", "userc@test.com", "Password@123", Role.User)
            };

            var query = new GetAllUserCaseQuery
            {
                PageNumber = 1,
                PageSize = 10
            };

            var expectedTuple = (
                Items: (IEnumerable<User>)users,
                TotalCount: users.Count
            );

            _userRepositoryMock
                .Setup(repo => repo.GetQueryableAllUsers(
                    null,
                    null,
                    query.PageNumber,
                    query.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTuple);

            // When
            var result = await _handler.Handle(query, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(3);
            result.TotalCount.Should().Be(3);
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(10);
        }
        [Fact]
        public async Task Given_NoUsersFound_When_HandlingQuery_Then_ShouldReturnEmptyPagedList()
        {
            // Given
            var query = new GetAllUserCaseQuery
            {
                PageNumber = 1,
                PageSize = 10
            };
        
            // Repositório retorna nenhum usuário
            var repositoryResponse = (
                Items: Enumerable.Empty<User>(),
                TotalCount: 0
            );
        
            _userRepositoryMock
                .Setup(repo => repo.GetQueryableAllUsers(
                    query.Email,
                    query.Role,
                    query.PageNumber,
                    query.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryResponse);
        
            // When
            var result = await _handler.Handle(query, CancellationToken.None);
        
            // Then
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty("pois o totalCount é zero");
            result.TotalCount.Should().Be(0, "pois nenhum usuário foi encontrado");
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(10);
        }
                [Fact]
        public async Task Given_EmailFilter_When_HandlingQuery_Then_ShouldReturnFilteredPagedList()
        {
            // Given
            var users = new List<User>
            {
                User.Create("User A", "usera@test.com", "Password@123", Role.User),
                User.Create("User B", "userb@test.com", "Password@123", Role.Admin)
            };

            var query = new GetAllUserCaseQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Email = "test.com"
            };

            var expectedUsers = users.Where(u => u.Email.Value.Contains("test.com")).ToList();
            var expectedTotalCount = expectedUsers.Count;

            var expectedTuple = (
                Items: (IEnumerable<User>)expectedUsers,
                TotalCount: expectedTotalCount
            );

            _userRepositoryMock
                .Setup(repo => repo.GetQueryableAllUsers(
                    query.Email,
                    query.Role,
                    query.PageNumber,
                    query.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTuple);

            // When
            var result = await _handler.Handle(query, CancellationToken.None);

            // Then
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(expectedTotalCount);
            result.Items.Should().HaveCount(expectedTotalCount);
        }
        [Fact]
        public async Task Given_PageNumber2_When_HandlingQuery_Then_ShouldReturnCorrectPage()
        {
            var totalUsersInDb = new List<User>
            {
                User.Create("User A", "usera@test.com", "Password@123", Role.User),
                User.Create("User B", "userb@test.com", "Password@123", Role.Admin),
                User.Create("User C", "userc@test.com", "Password@123", Role.User)
            };

            var query = new GetAllUserCaseQuery
            {
                PageNumber = 2,
                PageSize = 2
            };

            var usersForPage2 = totalUsersInDb.Skip(2).Take(2).ToList();
            var repositoryResponse = (
                Items: (IEnumerable<User>)usersForPage2,
                TotalCount: totalUsersInDb.Count
            );

            _userRepositoryMock
                .Setup(repo => repo.GetQueryableAllUsers(
                    null,
                    null,
                    query.PageNumber,
                    query.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryResponse);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.CurrentPage.Should().Be(2, "porque a página 2 foi solicitada");
            result.TotalCount.Should().Be(3, "o total de itens não muda com a paginação");
            result.Items.Should().HaveCount(1, "pois apenas um usuário sobrou para a segunda página");
            result.Items.First().Name.Should().Be("User C");
            result.TotalPages.Should().Be(2, "pois 3 itens divididos em páginas de 2 resultam em 2 páginas");
        }
        [Fact]
        public async Task Given_RoleFilter_When_HandlingQuery_Then_ShouldReturnFilteredPagedList()
        {

            var allUsers = new List<User>
            {
                User.Create("User A", "usera@test.com", "Password@123", Role.User),
                User.Create("User B", "userb@test.com", "Password@123", Role.Admin),
                User.Create("User C", "userc@test.com", "Password@123", Role.User)
            };

            var query = new GetAllUserCaseQuery
            {
                PageNumber = 1,
                PageSize = 10,
                Role = "Admin"
            };

            var expectedUsers = allUsers.Where(u => u.Role == Role.Admin).ToList();
            var repositoryResponse = (
                Items: (IEnumerable<User>)expectedUsers,
                TotalCount: expectedUsers.Count
            );

            _userRepositoryMock
                .Setup(repo => repo.GetQueryableAllUsers(
                    query.Email,
                    query.Role,
                    query.PageNumber,
                    query.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryResponse);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.TotalCount.Should().Be(1);
            result.Items.Should().HaveCount(1);
            result.Items.First().Role.Should().Be(Role.Admin.ToString());
        }
    }
}
