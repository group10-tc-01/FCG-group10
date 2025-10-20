using FCG.Application.UseCases.Users.Register;
using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Services
{
    public class RegisterUserUseCaseBuilder
    {
        private readonly Mock<IReadOnlyUserRepository> _readOnlyUserRepoMock = new();
        private readonly Mock<IWriteOnlyUserRepository> _writeOnlyUserRepoMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPasswordEncrypter> _passwordEncrypterMock = new();

        public RegisterUserUseCaseBuilder WithExistingEmail(string email)
        {
            _readOnlyUserRepoMock
                .Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
            return this;
        }

        public RegisterUserUseCaseBuilder WithNonExistingEmail(string email)
        {
            _readOnlyUserRepoMock
                .Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);
            return this;
        }

        public RegisterUserUseCaseBuilder WithPasswordEncryption(string encryptedPassword)
        {
            _passwordEncrypterMock
                .Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);
            return this;
        }

        public RegisterUserUseCase Build()
        {
            return new RegisterUserUseCase(
                _readOnlyUserRepoMock.Object,
                _writeOnlyUserRepoMock.Object,
                _unitOfWorkMock.Object,
                _passwordEncrypterMock.Object
            );
        }

        public Mock<IReadOnlyUserRepository> ReadOnlyRepoMock => _readOnlyUserRepoMock;
        public Mock<IWriteOnlyUserRepository> WriteOnlyRepoMock => _writeOnlyUserRepoMock;
        public Mock<IUnitOfWork> UnitOfWorkMock => _unitOfWorkMock;
        public Mock<IPasswordEncrypter> PasswordEncrypterMock => _passwordEncrypterMock;
    }
}