using FCG.Application.UseCases.Users.Register;
using FCG.CommomTestsUtilities.Builders.Inputs;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Repositories.WalletRepository;
using Moq;
using UoWBuilder = FCG.CommomTestsUtilities.Builders.Repositories.UnitOfWorkBuilder;

namespace FCG.FunctionalTests.Fixtures.Users
{
    public class RegisterUserFixture
    {
        public RegisterUserFixture()
        {
            var readOnlyUserRepository = ReadOnlyUserRepositoryBuilder.Build();
            var writeOnlyUserRepository = WriteOnlyUserRepositoryBuilder.Build();
            var writeOnlyWalletRepository = new Mock<IWriteOnlyWalletRepository>().Object;
            var writeOnlyLibraryRepository = new Mock<IWriteOnlyLibraryRepository>().Object;
            var unitOfWork = UoWBuilder.Build();
            var passwordEncrypter = PasswordEncrypterServiceBuilder.Build();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<RegisterUserUseCase>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            Setup();

            RegisterUserUseCase = new RegisterUserUseCase(
                readOnlyUserRepository,
                writeOnlyUserRepository,
                writeOnlyWalletRepository,
                writeOnlyLibraryRepository,
                unitOfWork,
                passwordEncrypter,
                logger,
                correlationIdProvider
            );
            RegisterUserRequest = CreateUserInputBuilder.Build();
        }

        public RegisterUserUseCase RegisterUserUseCase { get; }
        public RegisterUserRequest RegisterUserRequest { get; }

        private static void Setup()
        {
            ReadOnlyUserRepositoryBuilder.SetupGetByEmailAsync(null);
            WriteOnlyUserRepositoryBuilder.SetupAddAsync();
            PasswordEncrypterServiceBuilder.SetupEncrypt("EncryptedPass@123");
            UoWBuilder.SetupBeginTransactionAsync();
            UoWBuilder.SetupCommitAsync();
        }
    }
}
