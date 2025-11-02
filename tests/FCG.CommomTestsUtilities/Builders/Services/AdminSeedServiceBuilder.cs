using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Infrastructure.Services;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Services
{
    public class AdminSeedServiceBuilder
    {
        private readonly Mock<IReadOnlyUserRepository> _readOnlyUserRepoMock = new();
        private readonly Mock<IWriteOnlyUserRepository> _writeOnlyUserRepoMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IPasswordEncrypter> _passwordEncrypterMock = new();

        public AdminSeedServiceBuilder WithAdminAlreadyExisting()
        {
            _readOnlyUserRepoMock
                .Setup(x => x.AnyAdminAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            return this;
        }

        public AdminSeedServiceBuilder WithNoAdminExisting()
        {
            _readOnlyUserRepoMock
                .Setup(x => x.AnyAdminAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            return this;
        }

        public AdminSeedService Build()
        {
            _passwordEncrypterMock
                .Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns("Encrypted@123");

            return new AdminSeedService(
                _uowMock.Object,
                _writeOnlyUserRepoMock.Object,
                _readOnlyUserRepoMock.Object,
                _passwordEncrypterMock.Object
            );
        }

        public Mock<IReadOnlyUserRepository> ReadOnlyRepoMock => _readOnlyUserRepoMock;
        public Mock<IWriteOnlyUserRepository> WriteOnlyRepoMock => _writeOnlyUserRepoMock;
        public Mock<IUnitOfWork> UnitOfWorkMock => _uowMock;
    }
}
