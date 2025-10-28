using FCG.Application.UseCases.Admin.GetById;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.UseCases.AdminUsers.GetById
{
    public class GetByIdUserUseCaseTests
    {
        private readonly Mock<IReadOnlyUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<GetUserByIdUseCase>> _loggerMock;
        private readonly GetUserByIdUseCase _useCase;

        public GetByIdUserUseCaseTests()
        {
            _userRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _loggerMock = new Mock<ILogger<GetUserByIdUseCase>>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            _useCase = new GetUserByIdUseCase(_userRepositoryMock.Object, _loggerMock.Object, correlationIdProvider);
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