using FCG.Domain.Repositories;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Services
{
    public static class UnitOfWorkBuilder
    {
        private static readonly Mock<IUnitOfWork> _mock = new Mock<IUnitOfWork>();

        public static IUnitOfWork Build() => _mock.Object;

        public static void SetupSaveChangesAsync()
        {
            _mock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        }

        public static void VerifySaveChangesAsyncWasCalled()
        {
            _mock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}