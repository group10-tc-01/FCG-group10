using FCG.Application.UseCases.Admin.RoleManagement;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
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
            Setup(testUser);
            RoleManagementUseCase = new RoleManagementUseCase(readOnlyUserRepository, unitOfWork);
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
