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
        private readonly GetByIdUserUseCase _useCase;

        public GetByIdUserUseCaseTests()
        {
            _userRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _useCase = new GetByIdUserUseCase(_userRepositoryMock.Object);
        }

        [Fact(DisplayName = "Handle deve lançar NotFoundException quando usuário não existe")]
        public async Task Handle_GivenNonExistentUser_ShouldThrowNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetByIdUserQuery(userId);

            _userRepositoryMock.Setup(r => r.GetByIdWithDetailsAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // Act
            var act = () => _useCase.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Usuário com Id: {userId} não encontrado.");
        }

        [Fact(DisplayName = "Handle deve retornar resposta com Wallet nulo quando usuário não possui wallet")]
        public async Task Handle_GivenUserWithoutWallet_ShouldReturnResponseWithNullWallet()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetByIdUserQuery(userId);
            var user = UserBuilder.BuildWithData("Test User", "test@example.com", Role.User);

            _userRepositoryMock.Setup(r => r.GetByIdWithDetailsAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // Act
            var result = await _useCase.Handle(query, CancellationToken.None);

            // Assert
            result.Wallet.Should().BeNull();
            result.Library.Should().BeNull();
        }
    }
}