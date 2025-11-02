using FCG.Application.UseCases.Admin.CreateUser;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Enum;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Repositories.WalletRepository;
using Moq;
using UoWBuilder = FCG.CommomTestsUtilities.Builders.Repositories.UnitOfWorkBuilder;

namespace FCG.FunctionalTests.Fixtures.Admin
{
    public class CreateUserByAdminFixture
    {
        public CreateUserByAdminFixture()
        {
            var readOnlyUserRepository = ReadOnlyUserRepositoryBuilder.Build();
            var writeOnlyUserRepository = WriteOnlyUserRepositoryBuilder.Build();

            var writeOnlyWalletRepository = new Mock<IWriteOnlyWalletRepository>().Object;
            var writeOnlyLibraryRepository = new Mock<IWriteOnlyLibraryRepository>().Object;

            var unitOfWork = UoWBuilder.Build();
            var passwordEncrypter = PasswordEncrypterServiceBuilder.Build();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<CreateUserByAdminUseCase>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();

            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");
            PasswordEncrypterServiceBuilder.SetupEncrypt("hashedPassword123");

            SetupForNewUser();

            CreateUserByAdminUseCase = new CreateUserByAdminUseCase(
                readOnlyUserRepository,
                writeOnlyUserRepository,
                writeOnlyWalletRepository,
                writeOnlyLibraryRepository,
                unitOfWork,
                passwordEncrypter,
                logger,
                correlationIdProvider
            );

            CreateUserRequest = new CreateUserByAdminRequest
            {
                Name = "Test User",
                Email = "testuser@example.com",
                Password = "ValidP@ssw0rd!123",
                Role = Role.User
            };

            CreateAdminRequest = new CreateUserByAdminRequest
            {
                Name = "Test Admin",
                Email = "testadmin@example.com",
                Password = "AdminP@ssw0rd!123",
                Role = Role.Admin
            };
        }

        public CreateUserByAdminUseCase CreateUserByAdminUseCase { get; }
        public CreateUserByAdminRequest CreateUserRequest { get; }
        public CreateUserByAdminRequest CreateAdminRequest { get; }

        public void SetupForNewUser()
        {
            ReadOnlyUserRepositoryBuilder.SetupGetByEmailAsync(null);
            UoWBuilder.SetupBeginTransactionAsync();
            UoWBuilder.SetupCommitAsync();
            UoWBuilder.SetupRollbackAsync();
        }

        public void SetupForDuplicateEmail()
        {
            var existingUser = UserBuilder.BuildRegularUser();
            ReadOnlyUserRepositoryBuilder.SetupGetByEmailAsync(existingUser);
            UoWBuilder.SetupBeginTransactionAsync();
            UoWBuilder.SetupRollbackAsync();
        }
    }
}
