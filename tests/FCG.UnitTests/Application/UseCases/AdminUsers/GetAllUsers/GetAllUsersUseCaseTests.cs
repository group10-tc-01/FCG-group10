using FCG.Application.UseCases.Admin.GetAllUsers;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Repositories.UserRepository;
using FluentAssertions;
using Moq;

namespace FCG.UnitTests.Application.UseCases.AdminUsers.GetAllUsers
{
    public class GetAllUsersUseCaseTests
    {
        private readonly Mock<IReadOnlyUserRepository> _userRepositoryMock;
        private readonly GetAllUsersUseCase _useCase;

        public GetAllUsersUseCaseTests()
        {
            _userRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _useCase = new GetAllUsersUseCase(_userRepositoryMock.Object);
        }

        [Fact(DisplayName = "Handle deve retornar lista vazia quando não existem usuários")]
        public async Task Handle_GivenNoUsers_ShouldReturnEmptyPagedList()
        {
            // Arrange
            var request = new GetAllUserCaseRequest();
            _userRepositoryMock.Setup(r => r.GetQueryableAllUsers(
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync((new List<User>(), 0));

            // Act
            var result = await _useCase.Handle(request, CancellationToken.None);

            // Assert
            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
            result.CurrentPage.Should().Be(request.PageNumber);
            result.PageSize.Should().Be(request.PageSize);
        }

        [Fact(DisplayName = "Handle deve retornar lista paginada quando existem usuários")]
        public async Task Handle_GivenExistingUsers_ShouldReturnPagedListWithUsers()
        {
            // Arrange
            var request = new GetAllUserCaseRequest();
            var users = new List<User>
            {
                UserBuilder.BuildWithData("User1", "user1@test.com", Role.User),
                UserBuilder.BuildWithData("User2", "user2@test.com", Role.Admin)
            };

            _userRepositoryMock.Setup(r => r.GetQueryableAllUsers(
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync((users, 2));

            // Act
            var result = await _useCase.Handle(request, CancellationToken.None);

            // Assert
            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            result.Items[0].Name.Should().Be("User1");
            result.Items[0].Email.Should().Be("user1@test.com");
            result.Items[0].Role.Should().Be("User");
            result.Items[1].Name.Should().Be("User2");
            result.Items[1].Email.Should().Be("user2@test.com");
            result.Items[1].Role.Should().Be("Admin");
        }

        [Fact(DisplayName = "Handle deve mapear corretamente as propriedades do usuário")]
        public async Task Handle_ShouldMapUserPropertiesCorrectly()
        {
            // Arrange
            var request = new GetAllUserCaseRequest();
            var user = UserBuilder.BuildWithData("Test User", "test@example.com", Role.Admin);
            var users = new List<User> { user };

            _userRepositoryMock.Setup(r => r.GetQueryableAllUsers(
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync((users, 1));

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
    }
}