using FCG.Application.UseCases.Promotions.Create;
using FCG.Application.UseCases.Users.Update;
using FCG.CommomTestsUtilities.Builders.Inputs.Promotions.Create;
using FCG.CommomTestsUtilities.Builders.Repositories.GameRepository;
using FCG.CommomTestsUtilities.Builders.Repositories.PromotionRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.FunctionalTests.Fixtures.Promotions
{
    public class CreatePromotionFixture
    {
        public CreatePromotionFixture()
        {
            var readOnlyGameRepository = ReadOnlyGameRepositoryBuilder.Build();
            var readOnlyPromotionRepository = ReadOnlyPromotionRepositoryBuilder.Build();
            var writeOnlyPromotionRepository = WriteOnlyPromotionRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            var logger = new Mock<ILogger<UpdateUserUseCase>>().Object;

            Setup();

            CreatePromotionUseCase = new CreatePromotionUseCase(
                readOnlyGameRepository,
                readOnlyPromotionRepository,
                writeOnlyPromotionRepository,
                unitOfWork,
                correlationIdProvider,
                logger);
            CreatePromotionRequest = CreatePromotionInputBuilder.Build();
        }

        public CreatePromotionUseCase CreatePromotionUseCase { get; }
        public CreatePromotionRequest CreatePromotionRequest { get; }

        private static void Setup()
        {
            ReadOnlyGameRepositoryBuilder.SetupExistsAsync(true);
            ReadOnlyPromotionRepositoryBuilder.SetupExistsActivePromotionForGameAsync(false);
            WriteOnlyPromotionRepositoryBuilder.SetupAddAsync();
            UnitOfWorkBuilder.SetupSaveChangesAsync();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");
        }
    }
}
