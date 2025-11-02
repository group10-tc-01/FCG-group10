using FCG.Application.UseCases.Users.Update;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using UoWBuilder = FCG.CommomTestsUtilities.Builders.Repositories.UnitOfWorkBuilder;

namespace FCG.FunctionalTests.Fixtures.Users
{
    public class UpdateUserFixture
    {
        public UpdateUserFixture()
        {
            var readOnlyUserRepository = ReadOnlyUserRepositoryBuilder.Build();
            var unitOfWork = UoWBuilder.Build();
            var passwordEncrypter = PasswordEncrypterServiceBuilder.Build();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<UpdateUserUseCase>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            var loggedUser = LoggedUserServiceBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            var testUser = UserBuilder.Build();
            Setup(testUser);
            LoggedUserServiceBuilder.SetupGetLoggedUserAsync(testUser);

            UpdateUserUseCase = new UpdateUserUseCase(
                readOnlyUserRepository,
                unitOfWork,
                passwordEncrypter,
                logger,
                correlationIdProvider,
                loggedUser
            );

            UpdateUserRequest = new UpdateUserRequest
            {
                CurrentPassword = "OldPassword@123",
                NewPassword = "NewPassword@123"
            };
        }

        public UpdateUserUseCase UpdateUserUseCase { get; }
        public UpdateUserRequest UpdateUserRequest { get; }

        private static void Setup(FCG.Domain.Entities.User user)
        {
            ReadOnlyUserRepositoryBuilder.SetupGetByIdAsync(user);
            PasswordEncrypterServiceBuilder.SetupIsValid(true);
            PasswordEncrypterServiceBuilder.SetupEncrypt("Encrypted@123");
            UoWBuilder.SetupSaveChangesAsync();
        }
    }
}
