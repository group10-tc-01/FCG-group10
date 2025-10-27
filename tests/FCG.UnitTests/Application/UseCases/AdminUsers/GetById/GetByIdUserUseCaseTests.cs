using FCG.Application.UseCases.AdminUsers.GetById;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FluentAssertions;
using Moq;

namespace FCG.UnitTests.Application.UseCases.AdminUsers.GetById
{
    public class GetByIdUserUseCaseTests
    {
        private readonly Mock<IReadOnlyUserRepository> _userRepositoryMock;
        private readonly GetUserByIdUseCase _useCase;

        public GetByIdUserUseCaseTests()
        {
            _userRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _useCase = new GetUserByIdUseCase(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_GivenNonExistentUser_ShouldThrowNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserByIdRequest(userId);

            _userRepositoryMock.Setup(r => r.GetByIdWithDetailsAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var act = () => _useCase.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"User with Id: {userId} was not found.");
        }

        [Fact]
        public async Task Handle_GivenUserWithoutWallet_ShouldReturnResponseWithNullWallet()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserByIdRequest(userId);
            var user = UserBuilder.BuildWithData("Test User", "test@example.com", Role.User);

            _userRepositoryMock.Setup(r => r.GetByIdWithDetailsAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _useCase.Handle(query, CancellationToken.None);

            // Assert
            result.Wallet.Should().BeNull();
            result.Name.Should().Be("Test User");
            result.Email.Should().Be("test@example.com");
            result.Role.Should().Be(Role.User.ToString());
            result.Library.Should().BeNull();
        }
    }
}