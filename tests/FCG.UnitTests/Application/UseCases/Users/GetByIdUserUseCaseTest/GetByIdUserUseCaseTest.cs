using FCG.Application.UseCases.Admin.GetById;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FluentAssertions;
using Moq;

namespace FCG.UnitTests.Application.UseCases.Users.GetByIdUserUseCaseTest
{
    public class GetByIdUserUseCaseTest
    {
        private readonly Mock<IReadOnlyUserRepository> _userRepositoryMock;
        private readonly GetUserByIdUseCase _useCase;

        public GetByIdUserUseCaseTest()
        {
            _userRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _useCase = new GetUserByIdUseCase(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_GivenValidId_ShouldReturnUserData()
        {
            // Arrange
            var fakeUser = User.Create(
                "Usuário Válido",
                $"fake.{Guid.NewGuid()}@email.com",
                "SenhaForte123!",
                Role.User
            );

            var query = new GetUserByIdRequest(fakeUser.Id);

            _userRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(fakeUser.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeUser);

            // Act
            var result = await _useCase.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(fakeUser.Id);
            result.Name.Should().Be(fakeUser.Name.Value);
            result.Email.Should().Be(fakeUser.Email.Value);
            result.Role.Should().Be(fakeUser.Role.ToString());

            _userRepositoryMock.Verify(
                r => r.GetByIdWithDetailsAsync(fakeUser.Id, It.IsAny<CancellationToken>()),
                Times.Once
            );
        }
        [Theory]
        [InlineData("João", "joao@email.com")]
        [InlineData("Maria", "maria@email.com")]
        [InlineData("Pedro", "pedro@email.com")]
        public async Task Handle_WithDifferentUsers_ShouldReturnCorrectData(string name, string email)
        {
            // Arrange
            var user = User.Create(name, email, "Pass123!", Role.User);
            var query = new GetUserByIdRequest(user.Id);

            _userRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _useCase.Handle(query, CancellationToken.None);

            // Assert
            result.Name.Should().Be(name);
            result.Email.Should().Be(email);
        }

        [Fact]
        public async Task Handle_GivenUserBuiltByBuilder_ShouldReturnUserDetail()
        {
            // Arrange
            var user = UserBuilder.Build();
            var query = new GetUserByIdRequest(user.Id);

            _userRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _useCase.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id);
            result.Name.Should().Be(user.Name.Value);
            result.Email.Should().Be(user.Email.Value);
            result.Role.Should().Be(user.Role.ToString());
        }

        [Fact]
        public async Task Handle_GivenInvalidId_ShouldThrowNotFoundException()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var query = new GetUserByIdRequest(invalidId);

            _userRepositoryMock
                .Setup(r => r.GetByIdWithDetailsAsync(invalidId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act & Assert
            await _useCase.Invoking(u => u.Handle(query, CancellationToken.None))
                .Should().ThrowAsync<NotFoundException>()
                .WithMessage($"*{invalidId}*");
        }
    }
}
