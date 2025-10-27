using FCG.Domain.Entities;
using FCG.Domain.Repositories.PromotionRepository;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Repositories.PromotionRepository
{
    public static class ReadOnlyPromotionRepositoryBuilder
    {
        private static readonly Mock<IReadOnlyPromotionRepository> _mock = new Mock<IReadOnlyPromotionRepository>();

        public static IReadOnlyPromotionRepository Build() => _mock.Object;

        public static void SetupGetByIdAsync(Promotion? promotion)
        {
            _mock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(promotion);
        }

        public static void SetupGetByGameIdAsync(IEnumerable<Promotion> promotions)
        {
            _mock.Setup(repo => repo.GetByGameIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(promotions);
        }

        public static void SetupExistsActivePromotionForGameAsync(bool exists)
        {
            _mock.Setup(repo => repo.ExistsActivePromotionForGameAsync(
                It.IsAny<Guid>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(exists);
        }

        public static void VerifyExistsActivePromotionForGameAsyncWasCalled()
        {
            _mock.Verify(repo => repo.ExistsActivePromotionForGameAsync(
                It.IsAny<Guid>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>(),
                It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        public static void Reset()
        {
            _mock.Reset();
        }
    }
}
