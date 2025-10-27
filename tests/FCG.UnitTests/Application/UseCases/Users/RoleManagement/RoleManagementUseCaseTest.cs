using FCG.Application.UseCases.Users.RoleManagement;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Repositories;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Users.RoleManagement
{
    public class RoleManagementUseCaseTests
    {
        private static RoleManagementUseCase BuildUseCase(User user, out IWriteOnlyUserRepository writeOnlyRepo, out IUnitOfWork unitOfWork)
        {
            ReadOnlyUserRepositoryBuilder.SetupGetByIdAsync(user);
            var readOnlyRepo = ReadOnlyUserRepositoryBuilder.Build();

            WriteOnlyUserRepositoryBuilder.SetupUpdateAsync();
            writeOnlyRepo = WriteOnlyUserRepositoryBuilder.Build();

            UnitOfWorkBuilder.SetupSaveChangesAsync();
            unitOfWork = UnitOfWorkBuilder.Build();

            return new RoleManagementUseCase(readOnlyRepo, writeOnlyRepo, unitOfWork);
        }

        [Fact]
        public async Task ShouldPromoteRegularUserToAdmin()
        {
            // Arrange
            var user = UserBuilder.BuildRegularUser();
            var useCase = BuildUseCase(user, out var writeRepo, out var uow);
            var request = new RoleManagementRequest(user.Id, Role.Admin);

            // Act
            var result = await useCase.Handle(request, CancellationToken.None);

            // Assert
            user.Role.Should().Be(Role.Admin);
            result.Role.Should().Be(Role.Admin);
        }

        [Fact]
        public async Task ShouldDemoteAdminToUser()
        {
            // Arrange
            var user = UserBuilder.BuildAdmin();
            var useCase = BuildUseCase(user, out var writeRepo, out var uow);
            var request = new RoleManagementRequest(user.Id, Role.User);

            // Act
            var result = await useCase.Handle(request, CancellationToken.None);

            // Assert
            user.Role.Should().Be(Role.User);
            result.Role.Should().Be(Role.User);
        }
    }
}
