using FCG.Application.UseCases.Admin.RoleManagement;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Enum;
using UoWBuilder = FCG.CommomTestsUtilities.Builders.Repositories.UnitOfWorkBuilder;

namespace FCG.FunctionalTests.Fixtures.Admin
{
    public class RoleManagementFixture
    {
        public RoleManagementFixture()
        {
            var readOnlyUserRepository = ReadOnlyUserRepositoryBuilder.Build();
            var unitOfWork = UoWBuilder.Build();
            var testUser = UserBuilder.Build();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<RoleManagementUseCase>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            Setup(testUser);
            RoleManagementUseCase = new RoleManagementUseCase(readOnlyUserRepository, unitOfWork, logger, correlationIdProvider);
            RoleManagementRequest = new RoleManagementRequest(testUser.Id, Role.Admin);
        }

        public RoleManagementUseCase RoleManagementUseCase { get; }
        public RoleManagementRequest RoleManagementRequest { get; }

        private static void Setup(FCG.Domain.Entities.User user)
        {
            ReadOnlyUserRepositoryBuilder.SetupGetByIdAsync(user);
            UoWBuilder.SetupSaveChangesAsync();
        }
    }
}
