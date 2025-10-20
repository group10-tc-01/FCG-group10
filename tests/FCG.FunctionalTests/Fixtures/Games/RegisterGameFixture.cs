using FCG.Application.UseCases.Games.Register;
using FCG.CommomTestsUtilities.Builders.Inputs.Games.Register;
using FCG.CommomTestsUtilities.Builders.Repositories.GameRepository;
using FCG.CommomTestsUtilities.Builders.Services;

namespace FCG.FunctionalTests.Fixtures.Games
{
    public class RegisterGameFixture
    {
        public RegisterGameFixture()
        {
            var readOnlyGameRepository = ReadOnlyGameRepositoryBuilder.Build();
            var writeOnlyGameRepository = WriteOnlyGameRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();

            Setup();

            RegisterGameUseCase = new RegisterGameUseCase(writeOnlyGameRepository, readOnlyGameRepository, unitOfWork);
            RegisterGameInput = RegisterGameInputBuilder.Build();
        }

        public RegisterGameUseCase RegisterGameUseCase { get; }
        public RegisterGameInput RegisterGameInput { get; }

        private static void Setup()
        {
            ReadOnlyGameRepositoryBuilder.SetupGetByNameAsync(null);
            WriteOnlyGameRepositoryBuilder.SetupAddAsync();
            UnitOfWorkBuilder.SetupSaveChangesAsync();
        }
    }
}