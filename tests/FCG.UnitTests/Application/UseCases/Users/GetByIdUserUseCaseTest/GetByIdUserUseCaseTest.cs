using FCG.Application.UseCases.AdminUsers.GetAllUsers;
using FCG.Application.UseCases.AdminUsers.GetById;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Repositories.UserRepository;
using FluentAssertions;
using Moq;

namespace FCG.UnitTests.Application.UseCases.Users.GetByIdUserUseCaseTest
{
    public class GetByIdUserUseCaseTest
    {
        private readonly Mock<IReadOnlyUserRepository> _userRepositoryMock;
        private readonly GetByIdUserUseCase _useCase;

        public GetByIdUserUseCaseTest()
        {
            _userRepositoryMock = new Mock<IReadOnlyUserRepository>();

            _useCase = new GetByIdUserUseCase(_userRepositoryMock.Object);
        }
        [Fact]
        public async Task Handle_ShouldReturnUserDetail_WhenUserExists()
        {
            // Arrange
            var user = UserBuilder.Build(); // Cria um usuário válido
            var query = new GetByIdUserQuery(user.Id);

            _userRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _useCase.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Name.Value, result.Name);
            Assert.Equal(user.Email.Value, result.Email);
            Assert.Equal(user.Role.ToString(), result.Role);
        }
    }
}
