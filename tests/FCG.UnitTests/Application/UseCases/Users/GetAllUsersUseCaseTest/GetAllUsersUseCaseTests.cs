using FCG.Application.UseCases.Admin.GetAllUsers;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.CommomTestsUtilities.Extensions;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.UseCases.Users.GetAllUsersUseCaseTest
{
    public class GetAllUsersUseCaseTests
    {
        private readonly Mock<IReadOnlyUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<GetAllUsersUseCase>> _loggerMock;
        private readonly GetAllUsersUseCase _handler;

        public GetAllUsersUseCaseTests()
        {
            _userRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _loggerMock = new Mock<ILogger<GetAllUsersUseCase>>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            _handler = new GetAllUsersUseCase(_userRepositoryMock.Object, _loggerMock.Object, correlationIdProvider);
        }

        [Fact]
        public async Task Given_NoFilters_When_HandlingQuery_Then_ShouldReturnAllUsersPagedList()
        {
            // Arrange
            var users = new List<User>
            {
                User.Create(Name.Create("User A"), Email.Create("usera@test.com"), Password.Create("Password@123"), Role.User),
                User.Create(Name.Create("User B"), Email.Create("userb@test.com"), Password.Create("Password@123"), Role.Admin),
                User.Create(Name.Create("User C"), Email.Create("userc@test.com"), Password.Create("Password@123"), Role.User)
            };

            var query = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            SetupGetAllUsersWithFilters(users);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(3);
            result.TotalCount.Should().Be(3);
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(10);

            _userRepositoryMock.Verify(repo => repo.GetAllUsersWithFilters(
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task Given_NoUsersFound_When_HandlingQuery_Then_ShouldReturnEmptyPagedList()
        {
            // Arrange
            var query = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            SetupGetAllUsersWithFilters(Enumerable.Empty<User>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty("pois o totalCount é zero");
            result.TotalCount.Should().Be(0, "pois nenhum usuário foi encontrado");
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task Given_PageNumber2_When_HandlingQuery_Then_ShouldReturnCorrectPage()
        {
            // Arrange
            var totalUsersInDb = new List<User>
            {
                User.Create(Name.Create("User A"), Email.Create("usera@test.com"), Password.Create("Password@123"), Role.User),
                User.Create(Name.Create("User B"), Email.Create("userb@test.com"), Password.Create("Password@123"), Role.Admin),
                User.Create(Name.Create("User C"), Email.Create("userc@test.com"), Password.Create("Password@123"), Role.User)
            };

            var query = new GetAllUserCaseRequest
            {
                PageNumber = 2,
                PageSize = 2
            };

            SetupGetAllUsersWithFilters(totalUsersInDb);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.CurrentPage.Should().Be(2, "porque a página 2 foi solicitada");
            result.TotalCount.Should().Be(3, "o total de itens não muda com a paginação");
            result.Items.Should().HaveCount(1, "pois apenas um usuário sobrou para a segunda página");
            result.Items.First().Name.Should().Be("User C");
            result.TotalPages.Should().Be(2, "pois 3 itens divididos em páginas de 2 resultam em 2 páginas");
        }

        [Fact]
        public async Task Given_FilterByName_When_HandlingQuery_Then_ShouldReturnFilteredUsers()
        {
            // Arrange
            var users = new List<User>
            {
                User.Create(Name.Create("John Doe"), Email.Create("john@test.com"), Password.Create("Password@123"), Role.User),
                User.Create(Name.Create("Jane Smith"), Email.Create("jane@test.com"), Password.Create("Password@123"), Role.User),
                User.Create(Name.Create("John Smith"), Email.Create("johnsmith@test.com"), Password.Create("Password@123"), Role.Admin)
            };

            var filteredUsers = users.Where(u => u.Name.Value.Contains("John"));

            var query = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Name = "John"
            };

            SetupGetAllUsersWithFilters(filteredUsers);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.All(x => x.Name.Contains("John")).Should().BeTrue();
        }

        [Fact]
        public async Task Given_FilterByEmail_When_HandlingQuery_Then_ShouldReturnFilteredUsers()
        {
            // Arrange
            var users = new List<User>
            {
                User.Create(Name.Create("User A"), Email.Create("usera@test.com"), Password.Create("Password@123"), Role.User),
                User.Create(Name.Create("User B"), Email.Create("userb@example.com"), Password.Create("Password@123"), Role.Admin),
                User.Create(Name.Create("User C"), Email.Create("userc@test.com"), Password.Create("Password@123"), Role.User)
            };

            var filteredUsers = users.Where(u => u.Email.Value.Contains("test.com"));

            var query = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Email = "test.com"
            };

            SetupGetAllUsersWithFilters(filteredUsers);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.All(x => x.Email.Contains("test.com")).Should().BeTrue();
        }

        [Fact]
        public async Task Given_FilterByNameAndEmail_When_HandlingQuery_Then_ShouldReturnFilteredUsers()
        {
            // Arrange
            var users = new List<User>
            {
                User.Create(Name.Create("John Doe"), Email.Create("john@test.com"), Password.Create("Password@123"), Role.User),
                User.Create(Name.Create("John Smith"), Email.Create("johnsmith@example.com"), Password.Create("Password@123"), Role.Admin),
                User.Create(Name.Create("Jane Doe"), Email.Create("jane@test.com"), Password.Create("Password@123"), Role.User)
            };

            var filteredUsers = users.Where(u => u.Name.Value.Contains("John") && u.Email.Value.Contains("test.com"));

            var query = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Name = "John",
                Email = "test.com"
            };

            SetupGetAllUsersWithFilters(filteredUsers);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items.First().Name.Should().Be("John Doe");
            result.Items.First().Email.Should().Be("john@test.com");
        }

        private void SetupGetAllUsersWithFilters(IEnumerable<User> users)
        {
            var queryable = users.AsQueryable().BuildMockDbSet();
            _userRepositoryMock.Setup(r => r.GetAllUsersWithFilters(
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns(queryable);
        }
    }
}
