using FCG.Application.Services.Seeds;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Services
{
    public class AdminSeedServiceBuilder
    {
        private readonly Mock<IReadOnlyUserRepository> _readOnlyUserRepoMock = new();
        private readonly Mock<IWriteOnlyUserRepository> _writeOnlyUserRepoMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();

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
            return new AdminSeedService(
                _uowMock.Object,
                _writeOnlyUserRepoMock.Object,
                _readOnlyUserRepoMock.Object
            );
        }

        public Mock<IReadOnlyUserRepository> ReadOnlyRepoMock => _readOnlyUserRepoMock;
        public Mock<IWriteOnlyUserRepository> WriteOnlyRepoMock => _writeOnlyUserRepoMock;
        public Mock<IUnitOfWork> UnitOfWorkMock => _uowMock;
    }
}
