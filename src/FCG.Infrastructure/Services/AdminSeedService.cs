using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;

namespace FCG.Infrastructure.Services
{
    public class AdminSeedService : IAdminSeedService
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IWriteOnlyUserRepository _writeOnlyUserRepository;
        private readonly IUnitOfWork _uow;
        private readonly IPasswordEncrypter _passwordEncrypter;

        public AdminSeedService(IUnitOfWork uow,
            IWriteOnlyUserRepository writeOnlyUserRepository, IReadOnlyUserRepository readOnlyUserRepository, IPasswordEncrypter passwordEncrypter)
        {
            _uow = uow;
            _writeOnlyUserRepository = writeOnlyUserRepository;
            _readOnlyUserRepository = readOnlyUserRepository;
            _passwordEncrypter = passwordEncrypter;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            var hasAnyAdmin = await _readOnlyUserRepository.AnyAdminAsync(cancellationToken);
            if (!hasAnyAdmin)
            {
                var password = _passwordEncrypter.Encrypt("Admin@123");
                var admin = User.Create("admin", "admin@mail.com", password, Role.Admin);
                var wallet = Wallet.Create(admin.Id);
                await _writeOnlyUserRepository.AddAsync(admin);
                await _uow.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
