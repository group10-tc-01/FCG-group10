using FCG.Application.UseCases.Admin.GetAllUsers;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.CommomTestsUtilities.Extensions;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Repositories.UserRepository;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.UseCases.Admin.GetAllUsers
{
    public class GetAllUsersUseCaseTests
    {
        private readonly Mock<IReadOnlyUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<GetAllUsersUseCase>> _loggerMock;
        private readonly GetAllUsersUseCase _useCase;

        public GetAllUsersUseCaseTests()
        {
            _userRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _loggerMock = new Mock<ILogger<GetAllUsersUseCase>>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            _useCase = new GetAllUsersUseCase(_userRepositoryMock.Object, _loggerMock.Object, correlationIdProvider);
        }

        [Fact]
        public async Task Given_NoUsers_When_HandlingRequest_Then_ReturnsEmptyPagedList()
        {
            // Arrange
            var request = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            var emptyUsers = Enumerable.Empty<User>();
            SetupGetAllUsersWithFilters(emptyUsers);

            // Act
            var result = await _useCase.Handle(request, CancellationToken.None);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.CurrentPage.Should().Be(request.PageNumber);
            result.PageSize.Should().Be(request.PageSize);
        }

        [Fact]
        public async Task Given_ExistingUsers_When_HandlingRequest_Then_ReturnsPagedListWithUsers()
        {
            // Arrange
            var request = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            var users = new List<User>
            {
                UserBuilder.BuildWithData("User1", "user1@test.com", Role.User),
                UserBuilder.BuildWithData("User2", "user2@test.com", Role.Admin)
            };

            SetupGetAllUsersWithFilters(users);

            // Act
            var result = await _useCase.Handle(request, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
        }

        [Fact]
        public async Task Given_ExistingUser_When_HandlingRequest_Then_MapsUserPropertiesCorrectly()
        {
            // Arrange
            var request = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            var user = UserBuilder.BuildWithData("Test User", "test@example.com", Role.Admin);
            var users = new List<User> { user };

            SetupGetAllUsersWithFilters(users);

            // Act
            var result = await _useCase.Handle(request, CancellationToken.None);

            // Assert
            var userResponse = result.Items.First();
            userResponse.Id.Should().Be(user.Id);
            userResponse.Name.Should().Be(user.Name.Value);
            userResponse.Email.Should().Be(user.Email.Value);
            userResponse.Role.Should().Be(user.Role.ToString());
            userResponse.CreatedAt.Should().Be(user.CreatedAt);
        }

        [Fact]
        public async Task Given_FilterByName_When_HandlingRequest_Then_ReturnsFilteredUsers()
        {
            // Arrange
            var request = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Name = "John"
            };

            var users = new List<User>
            {
                UserBuilder.BuildWithData("John Doe", "john@test.com", Role.User),
                UserBuilder.BuildWithData("John Smith", "johnsmith@test.com", Role.User)
            };

            SetupGetAllUsersWithFilters(users);

            // Act
            var result = await _useCase.Handle(request, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(2);
            result.Items.All(x => x.Name.Contains("John")).Should().BeTrue();
        }

        [Fact]
        public async Task Given_FilterByEmail_When_HandlingRequest_Then_ReturnsFilteredUsers()
        {
            // Arrange
            var request = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10,
                Email = "test.com"
            };

            var users = new List<User>
            {
                UserBuilder.BuildWithData("User1", "user1@test.com", Role.User),
                UserBuilder.BuildWithData("User2", "user2@test.com", Role.Admin)
            };

            SetupGetAllUsersWithFilters(users);

            // Act
            var result = await _useCase.Handle(request, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(2);
            result.Items.All(x => x.Email.Contains("test.com")).Should().BeTrue();
        }

        [Fact]
        public async Task Given_PaginationParams_When_HandlingRequest_Then_ReturnsCorrectPage()
        {
            // Arrange
            var request = new GetAllUserCaseRequest
            {
                PageNumber = 2,
                PageSize = 5
            };

            var users = UserBuilder.BuildList(12);
            SetupGetAllUsersWithFilters(users);

            // Act
            var result = await _useCase.Handle(request, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(5);
            result.TotalCount.Should().Be(12);
            result.CurrentPage.Should().Be(2);
            result.PageSize.Should().Be(5);
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
