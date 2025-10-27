using FCG.Application.UseCases.Users.Register;
using FCG.Domain.Entities;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Repositories.WalletRepository;
using FCG.Domain.Services;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Services
{
    public class RegisterUserUseCaseBuilder
    {
        private readonly Mock<IReadOnlyUserRepository> _readOnlyUserRepoMock = new();
        private readonly Mock<IWriteOnlyUserRepository> _writeOnlyUserRepoMock = new();
        private readonly Mock<IWriteOnlyWalletRepository> _writeOnlyWalletRepoMock = new();
        private readonly Mock<IWriteOnlyLibraryRepository> _writeOnlyLibraryRepoMock = new();
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

        public RegisterUserUseCaseBuilder WithSuccessfulWalletCreation()
        {
            _writeOnlyWalletRepoMock
                .Setup(x => x.AddAsync(It.IsAny<Wallet>()))
                .Returns(Task.CompletedTask);
            return this;
        }

        public RegisterUserUseCaseBuilder WithSuccessfulLibraryCreation()
        {
            _writeOnlyLibraryRepoMock
                .Setup(x => x.AddAsync(It.IsAny<Library>()))
                .Returns(Task.CompletedTask);
            return this;
        }

        public RegisterUserUseCaseBuilder WithSuccessfulUserCreation()
        {
            _writeOnlyUserRepoMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);
            return this;
        }

        public RegisterUserUseCaseBuilder WithSuccessfulSaveChanges()
        {
            _unitOfWorkMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
            return this;
        }

        public RegisterUserUseCase Build()
        {
            return new RegisterUserUseCase(
                _readOnlyUserRepoMock.Object,
                _writeOnlyUserRepoMock.Object,
                _writeOnlyWalletRepoMock.Object,
                _writeOnlyLibraryRepoMock.Object,
                _unitOfWorkMock.Object,
                _passwordEncrypterMock.Object
            );
        }

        public Mock<IReadOnlyUserRepository> ReadOnlyRepoMock => _readOnlyUserRepoMock;
        public Mock<IWriteOnlyUserRepository> WriteOnlyRepoMock => _writeOnlyUserRepoMock;
        public Mock<IWriteOnlyWalletRepository> WriteOnlyWalletRepoMock => _writeOnlyWalletRepoMock;
        public Mock<IWriteOnlyLibraryRepository> WriteOnlyLibraryRepoMock => _writeOnlyLibraryRepoMock;
        public Mock<IUnitOfWork> UnitOfWorkMock => _unitOfWorkMock;
        public Mock<IPasswordEncrypter> PasswordEncrypterMock => _passwordEncrypterMock;
    }
}