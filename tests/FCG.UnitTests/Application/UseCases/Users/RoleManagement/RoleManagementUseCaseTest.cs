using FCG.Application.UseCases.Admin.RoleManagement;
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
        private static RoleManagementUseCase BuildUseCase(User user, out IUnitOfWork unitOfWork)
        {
            ReadOnlyUserRepositoryBuilder.SetupGetByIdAsync(user);
            var readOnlyRepo = ReadOnlyUserRepositoryBuilder.Build();

            UnitOfWorkBuilder.SetupSaveChangesAsync();
            unitOfWork = UnitOfWorkBuilder.Build();

            return new RoleManagementUseCase(readOnlyRepo, unitOfWork);
        }

        [Fact]
        public async Task Given_RegularUser_When_PromoteToAdmin_Then_ShouldUpdateRole()
        {
            // Arrange
            var user = UserBuilder.BuildRegularUser();
            var useCase = BuildUseCase(user, out var uow);
            var request = new RoleManagementRequest(user.Id, Role.Admin);

            // Act
            var result = await useCase.Handle(request, CancellationToken.None);

            // Assert
            user.Role.Should().Be(Role.Admin);
            result.Role.Should().Be(Role.Admin);
        }

        [Fact]
        public async Task Given_AdminUser_When_DemoteToUser_Then_ShouldUpdateRole()
        {
            // Arrange
            var user = UserBuilder.BuildAdmin();
            var useCase = BuildUseCase(user, out var uow);
            var request = new RoleManagementRequest(user.Id, Role.User);

            // Act
            var result = await useCase.Handle(request, CancellationToken.None);

            // Assert
            user.Role.Should().Be(Role.User);
            result.Role.Should().Be(Role.User);
        }
    }
}
