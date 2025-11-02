using FCG.Application.UseCases.Users.Update;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Messages;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.UseCases.Users.Update
{
    public class UpdateUserUseCaseTests
    {
        private readonly Mock<IReadOnlyUserRepository> _readOnlyUserRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPasswordEncrypter> _passwordEncrypterMock;
        private readonly Mock<ILogger<UpdateUserUseCase>> _loggerMock;
        private readonly Mock<ILoggedUser> _loggedUserMock;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly UpdateUserUseCase _useCase;

        public UpdateUserUseCaseTests()
        {
            _readOnlyUserRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _passwordEncrypterMock = new Mock<IPasswordEncrypter>();
            _loggerMock = new Mock<ILogger<UpdateUserUseCase>>();
            _loggedUserMock = new Mock<ILoggedUser>();
            _correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            _useCase = new UpdateUserUseCase(
                _readOnlyUserRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _passwordEncrypterMock.Object,
                _loggerMock.Object,
                _correlationIdProvider,
                _loggedUserMock.Object
            );
        }

        [Fact]
        public async Task Given_NonExistentUser_When_UpdatePassword_Then_ShouldThrowNotFoundException()
        {
            // Arrange
            var user = UserBuilder.Build();
            var request = new UpdateUserRequest
            {
                CurrentPassword = "OldPass@123",
                NewPassword = "NewPass@456"
            };

            _loggedUserMock
                .Setup(x => x.GetLoggedUserAsync())
                .ThrowsAsync(new NotFoundException("User not found"));

            // Act
            var act = async () => await _useCase.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Given_IncorrectCurrentPassword_When_UpdatePassword_Then_ShouldThrowDomainException()
        {
            // Arrange
            var user = UserBuilder.Build();
            var request = new UpdateUserRequest
            {
                CurrentPassword = "WrongPass@123",
                NewPassword = "NewPass@456"
            };

            _loggedUserMock
                .Setup(x => x.GetLoggedUserAsync())
                .ReturnsAsync(user);

            _passwordEncrypterMock
                .Setup(x => x.IsValid("WrongPass@123", user.Password.Value))
                .Returns(false);

            // Act
            var act = async () => await _useCase.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage(ResourceMessages.CurrentPasswordIncorrect);
        }

        [Fact]
        public async Task Given_SameCurrentAndNewPassword_When_UpdatePassword_Then_ShouldThrowDomainException()
        {
            // Arrange
            var user = UserBuilder.Build();
            var request = new UpdateUserRequest
            {
                CurrentPassword = "SamePass@123",
                NewPassword = "SamePass@123"
            };

            _loggedUserMock
                .Setup(x => x.GetLoggedUserAsync())
                .ReturnsAsync(user);

            _passwordEncrypterMock
                .Setup(x => x.IsValid("SamePass@123", user.Password.Value))
                .Returns(true);

            // Act
            var act = async () => await _useCase.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Given_ValidRequest_When_UpdatePassword_Then_ShouldCallRepositoryInCorrectOrder()
        {
            // Arrange
            var user = UserBuilder.Build();
            var newHashedPassword = "new_hashed_password";
            var request = new UpdateUserRequest
            {
                CurrentPassword = "OldPass@123",
                NewPassword = "NewPass@456"
            };

            _loggedUserMock
                .Setup(x => x.GetLoggedUserAsync())
                .ReturnsAsync(user);

            _passwordEncrypterMock
                .Setup(x => x.IsValid("OldPass@123", user.Password.Value))
                .Returns(true);

            _passwordEncrypterMock
                .Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(newHashedPassword);

            var callOrder = new List<string>();

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Callback(() => callOrder.Add("SaveChangesAsync"))
                .ReturnsAsync(1);

            // Act
            await _useCase.Handle(request, CancellationToken.None);

            // Assert
            callOrder.Should().HaveCount(1);
            callOrder[0].Should().Be("SaveChangesAsync");

            _passwordEncrypterMock.Verify(x => x.IsValid(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _passwordEncrypterMock.Verify(x => x.Encrypt(It.IsAny<string>()), Times.Once);
        }
    }
}