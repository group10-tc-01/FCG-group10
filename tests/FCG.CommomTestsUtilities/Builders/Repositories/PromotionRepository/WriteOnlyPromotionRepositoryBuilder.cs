using FCG.Domain.Entities;
using FCG.Domain.Repositories.PromotionRepository;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Repositories.PromotionRepository
{
    public static class WriteOnlyPromotionRepositoryBuilder
    {
        private static readonly Mock<IWriteOnlyPromotionRepository> _mock = new Mock<IWriteOnlyPromotionRepository>();

        public static IWriteOnlyPromotionRepository Build() => _mock.Object;

        public static void SetupAddAsync()
        {
            _mock.Setup(repo => repo.AddAsync(It.IsAny<Promotion>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        }

        public static void SetupUpdateAsync()
        {
            _mock.Setup(repo => repo.UpdateAsync(It.IsAny<Promotion>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        }

        public static void SetupDeleteAsync()
        {
            _mock.Setup(repo => repo.DeleteAsync(It.IsAny<Promotion>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        }

        public static void VerifyAddAsyncWasCalled()
        {
            _mock.Verify(repo => repo.AddAsync(It.IsAny<Promotion>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        public static void VerifyUpdateAsyncWasCalled()
        {
            _mock.Verify(repo => repo.UpdateAsync(It.IsAny<Promotion>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        public static void VerifyDeleteAsyncWasCalled()
        {
            _mock.Verify(repo => repo.DeleteAsync(It.IsAny<Promotion>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        public static void Reset()
        {
            _mock.Reset();
        }
    }
}
