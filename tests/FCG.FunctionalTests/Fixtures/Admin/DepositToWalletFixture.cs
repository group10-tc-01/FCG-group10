using FCG.Application.UseCases.Admin.DepositToWallet;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Repositories.WalletRepository;
using Moq;
using UoWBuilder = FCG.CommomTestsUtilities.Builders.Repositories.UnitOfWorkBuilder;

namespace FCG.FunctionalTests.Fixtures.Admin
{
    public class DepositToWalletFixture
    {
        private Wallet? _wallet;

        public DepositToWalletFixture()
        {
            var readOnlyWalletRepositoryMock = new Mock<IReadOnlyWalletRepository>();
            ReadOnlyWalletRepository = readOnlyWalletRepositoryMock.Object;

            var unitOfWork = UoWBuilder.Build();
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<DepositToWalletUseCase>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();

            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");
            UoWBuilder.SetupBeginTransactionAsync();
            UoWBuilder.SetupCommitAsync();
            UoWBuilder.SetupRollbackAsync();

            DepositToWalletUseCase = new DepositToWalletUseCase(
                ReadOnlyWalletRepository,
                unitOfWork,
                logger,
                correlationIdProvider
            );

            _wallet = WalletBuilder.Build();

            DepositRequest = new DepositToWalletRequest
            {
                UserId = _wallet.UserId,
                Amount = 100.00m
            };
        }

        public DepositToWalletUseCase DepositToWalletUseCase { get; }
        public DepositToWalletRequest DepositRequest { get; }
        public IReadOnlyWalletRepository ReadOnlyWalletRepository { get; }

        public void SetupForExistingWallet()
        {
            Mock.Get(ReadOnlyWalletRepository)
                .Setup(x => x.GetByUserIdAsync(_wallet!.UserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_wallet);
        }

        public void SetupForNonExistentWallet()
        {
            Mock.Get(ReadOnlyWalletRepository)
                .Setup(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Wallet?)null);
        }

        public decimal GetInitialBalance()
        {
            return _wallet?.Balance ?? 0;
        }
    }
}
