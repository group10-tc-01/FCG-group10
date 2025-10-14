using FCG.Domain.Repositories;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace FCG.CommomTestsUtilities.Builders.Repositories
{

    public class UnitOfWorkBuilder
    {
        private static readonly Mock<IUnitOfWork> _mock = new Mock<IUnitOfWork>();

        public static IUnitOfWork Build() => _mock.Object;

        public static void SetupBeginTransactionAsync()
        {
            _mock.Setup(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);
        }

        public static void SetupSaveChangesAsync(int returnValue = 1)
        {
            _mock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(returnValue);
        }

        public static void SetupCommitAsync(int returnValue = 1)
        {
            _mock.Setup(uow => uow.CommitAsync(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(returnValue);
        }

        public static void SetupRollbackAsync()
        {
            _mock.Setup(uow => uow.RollbackAsync(It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);
        }

        public static void SetupAllMethods(int saveReturnValue = 1, int commitReturnValue = 1)
        {
            SetupBeginTransactionAsync();
            SetupSaveChangesAsync(saveReturnValue);
            SetupCommitAsync(commitReturnValue);
            SetupRollbackAsync();
        }

        public static void Reset()
        {
            _mock.Reset();
        }
    }
}