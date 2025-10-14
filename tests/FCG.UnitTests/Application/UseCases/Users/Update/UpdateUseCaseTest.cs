﻿using FCG.Application.UseCases.Users.Update;
using FCG.Application.UseCases.Users.Update.UsersDTO;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Domain.ValueObjects;
using Moq;

namespace FCG.Tests.Application.UseCases.Users
{
    public class UpdateUserUseCaseTests
    {
        private readonly Mock<IReadOnlyUserRepository> _readOnlyUserRepositoryMock;
        private readonly Mock<IWriteOnlyUserRepository> _writeOnlyUserRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPasswordEncrypter> _passwordEncrypterMock;
        private readonly UpdateUserUseCase _useCase;

        public UpdateUserUseCaseTests()
        {
            _readOnlyUserRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _writeOnlyUserRepositoryMock = new Mock<IWriteOnlyUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _passwordEncrypterMock = new Mock<IPasswordEncrypter>();

            _useCase = new UpdateUserUseCase(
                _readOnlyUserRepositoryMock.Object,
                _writeOnlyUserRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _passwordEncrypterMock.Object
            );
        }

        [Fact]
        public async Task Given_NonExistentUser_When_UpdatePassword_Then_ShouldThrowNotFoundException()
        {
            // Given
            var userId = Guid.NewGuid();
            var request = new UpdateUserRequest
            {
                Id = userId,
                CurrentPassword = "OldPass@123",
                NewPassword = "NewPass@456"
            };

            _readOnlyUserRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            // When
            var act = () => _useCase.Handle(request, CancellationToken.None);

            // Then
            var exception = await Assert.ThrowsAsync<NotFoundException>(act);
            Assert.Contains(userId.ToString(), exception.Message);
            Assert.Contains("não encontrado", exception.Message);
        }

        [Fact]
        public async Task Given_EmptyCurrentPassword_When_UpdatePassword_Then_ShouldThrowDomainException()
        {
            // Given
            var userId = Guid.NewGuid();
            var hashedPassword = "hashed_old_password";
            var user = CreateUserWithHashedPassword(userId, hashedPassword);

            var request = new UpdateUserRequest
            {
                Id = userId,
                CurrentPassword = "",
                NewPassword = "NewPass@456"
            };

            _readOnlyUserRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            // When
            var act = () => _useCase.Handle(request, CancellationToken.None);

            // Then
            var exception = await Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal("A senha atual é obrigatória para alterar a senha.", exception.Message);
        }

        [Fact]
        public async Task Given_IncorrectCurrentPassword_When_UpdatePassword_Then_ShouldThrowDomainException()
        {
            // Given
            var userId = Guid.NewGuid();
            var correctHashedPassword = "correct_hashed_password";
            var incorrectHashedPassword = "incorrect_hashed_password";
            var user = CreateUserWithHashedPassword(userId, correctHashedPassword);

            var request = new UpdateUserRequest
            {
                Id = userId,
                CurrentPassword = "WrongPass@123",
                NewPassword = "NewPass@456"
            };

            _readOnlyUserRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordEncrypterMock
                .Setup(x => x.Encrypt("WrongPass@123"))
                .Returns(incorrectHashedPassword);

            // When
            var act = () => _useCase.Handle(request, CancellationToken.None);

            // Then
            var exception = await Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal("A senha atual está incorreta.", exception.Message);
        }

        [Fact]
        public async Task Given_SameCurrentAndNewPassword_When_UpdatePassword_Then_ShouldThrowDomainException()
        {
            // Given
            var userId = Guid.NewGuid();
            var hashedPassword = "hashed_same_password";
            var user = CreateUserWithHashedPassword(userId, hashedPassword);

            var request = new UpdateUserRequest
            {
                Id = userId,
                CurrentPassword = "SamePass@123",
                NewPassword = "SamePass@123"
            };

            _readOnlyUserRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordEncrypterMock
                .Setup(x => x.IsValid("SamePass@123", hashedPassword))
                .Returns(true);

            // When
            var act = () => _useCase.Handle(request, CancellationToken.None);

            // Then
            var exception = await Assert.ThrowsAsync<DomainException>(act);
            Assert.Equal("A nova senha deve ser diferente da senha atual.", exception.Message);
        }

        [Fact]
        public async Task Given_ValidRequest_When_UpdatePassword_Then_ShouldCallRepositoryInCorrectOrder()
        {
            var userId = Guid.NewGuid();
            var oldHashedPassword = "old_hashed_password";
            var newHashedPassword = "new_hashed_password";
            var user = CreateUserWithHashedPassword(userId, oldHashedPassword);
            var request = new UpdateUserRequest
            {
                Id = userId,
                CurrentPassword = "OldPass@123",
                NewPassword = "NewPass@456"
            };

            _readOnlyUserRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _passwordEncrypterMock
                .Setup(x => x.IsValid("OldPass@123", oldHashedPassword))
                .Returns(true); // ⬅️ TRUE para que a validação passe

            _passwordEncrypterMock
                .Setup(x => x.Encrypt("NewPass@456"))
                .Returns(newHashedPassword);

            var callOrder = new List<string>();

            _writeOnlyUserRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .Callback(() => callOrder.Add("UpdateAsync"))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Callback(() => callOrder.Add("SaveChangesAsync"))
                .ReturnsAsync(1); // Retorna 1 para sucesso

            // When (ACT)
            await _useCase.Handle(request, CancellationToken.None);

            // Then (ASSERT)
            Assert.Equal(2, callOrder.Count);
            Assert.Equal("UpdateAsync", callOrder[0]);
            Assert.Equal("SaveChangesAsync", callOrder[1]);

            _passwordEncrypterMock.Verify(x => x.IsValid("OldPass@123", oldHashedPassword), Times.Once);
            _passwordEncrypterMock.Verify(x => x.Encrypt("NewPass@456"), Times.Once);
        }
        private User CreateUserWithHashedPassword(Guid userId, string hashedPassword)
        {
            var user = User.Create(
                "Test User",
                "test@example.com",
                "TempPass@123",
                FCG.Domain.Enum.Role.User
            );

            var idProperty = typeof(User).BaseType?.GetProperty("Id");
            idProperty?.SetValue(user, userId);

            var passwordField = typeof(User).GetProperty("Password");
            passwordField?.SetValue(user, Password.CreateFromHash(hashedPassword));

            return user;
        }
    }
}