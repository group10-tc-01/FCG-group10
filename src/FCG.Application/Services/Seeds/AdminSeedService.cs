using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;

namespace FCG.Application.Services.Seeds
{
    public class AdminSeedService : ISeed
    {
        private readonly IReadOnlyUserRepository _readOnlyUserRepository;
        private readonly IWriteOnlyUserRepository _writeOnlyUserRepository;
        private readonly IUnitOfWork _uow;

        public AdminSeedService(IUnitOfWork uow, IWriteOnlyUserRepository writeOnlyUserRepository, IReadOnlyUserRepository readOnlyUserRepository)
        {
            _uow = uow;
            _writeOnlyUserRepository = writeOnlyUserRepository;
            _readOnlyUserRepository = readOnlyUserRepository;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            var hasAnyAdmin = await _readOnlyUserRepository.AnyAdminAsync(cancellationToken);
            if (!hasAnyAdmin)
            {
                var admin = User.Create("admin", "admin@mail.com", "admin@123", Role.Admin);
                var wallet = Wallet.Create(admin.Id);
                await _writeOnlyUserRepository.AddAsync(admin, wallet);
                await _uow.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
