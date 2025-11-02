using FCG.Application.UseCases.Admin.DepositToWallet;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Inputs.Admin.DepositToWallet;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Repositories.WalletRepository;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using UoWBuilder = FCG.CommomTestsUtilities.Builders.Repositories.UnitOfWorkBuilder;

namespace FCG.UnitTests.Application.UseCases.Admin.DepositToWallet
{
    public class DepositToWalletUseCaseTest
    {
        private static DepositToWalletUseCase BuildUseCase(
            out IReadOnlyUserRepository readOnlyUserRepository,
            out IReadOnlyWalletRepository readOnlyWalletRepository,
            out IUnitOfWork unitOfWork,
            User? existingUser = null,
            Wallet? existingWallet = null)
        {
            var readOnlyUserRepoMock = new Mock<IReadOnlyUserRepository>();
            if (existingUser != null)
            {
                readOnlyUserRepoMock
                    .Setup(x => x.GetByIdAsync(existingUser.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(existingUser);
            }
            else
            {
                readOnlyUserRepoMock
                    .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((User?)null);
            }
            readOnlyUserRepository = readOnlyUserRepoMock.Object;

            var readOnlyWalletRepoMock = new Mock<IReadOnlyWalletRepository>();
            if (existingWallet != null)
            {
                readOnlyWalletRepoMock
                    .Setup(x => x.GetByIdAsync(existingWallet.Id, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(existingWallet);
            }
            else
            {
                readOnlyWalletRepoMock
                    .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Wallet?)null);
            }
            readOnlyWalletRepository = readOnlyWalletRepoMock.Object;

            UoWBuilder.SetupBeginTransactionAsync();
            UoWBuilder.SetupCommitAsync();
            UoWBuilder.SetupRollbackAsync();
            unitOfWork = UoWBuilder.Build();

            var logger = new Mock<ILogger<DepositToWalletUseCase>>().Object;
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();

            return new DepositToWalletUseCase(
                readOnlyUserRepository,
                readOnlyWalletRepository,
                unitOfWork,
                logger,
                correlationIdProvider
            );
        }

        [Fact]
        public async Task Given_ValidRequest_When_DepositingToWallet_Then_ShouldIncreaseBalanceSuccessfully()
        {
            // Arrange
            var user = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user.Id);
            var initialBalance = wallet.Balance;
            var depositAmount = 100.00m;

            var request = DepositToWalletInputBuilder.BuildWithUserIdAndWalletId(user.Id, wallet.Id);

            var useCase = BuildUseCase(out _, out _, out var unitOfWork, user, wallet);

            // Act
            var result = await useCase.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(user.Id);
            result.DepositedAmount.Should().Be(depositAmount);
            result.NewBalance.Should().Be(initialBalance + depositAmount);
        }

        [Fact]
        public async Task Given_UserNotFound_When_DepositingToWallet_Then_ShouldThrowNotFoundException()
        {
            // Arrange
            var request = DepositToWalletInputBuilder.Build();
            var useCase = BuildUseCase(out _, out _, out var unitOfWork, null, null);

            // Act
            Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"User not found with ID: {request.UserId}");
        }

        [Fact]
        public async Task Given_WalletNotFound_When_DepositingToWallet_Then_ShouldThrowNotFoundException()
        {
            // Arrange
            var user = UserBuilder.Build();
            var request = DepositToWalletInputBuilder.BuildWithUserIdAndWalletId(user.Id, Guid.NewGuid());
            var useCase = BuildUseCase(out _, out _, out var unitOfWork, user, null);

            // Act
            Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Wallet not found with ID: {request.WalletId}");
        }

        [Fact]
        public async Task Given_WalletDoesNotBelongToUser_When_DepositingToWallet_Then_ShouldThrowDomainException()
        {
            // Arrange
            var user = UserBuilder.Build();
            var wallet = WalletBuilder.Build(); // Wallet de outro usuário
            var request = DepositToWalletInputBuilder.BuildWithUserIdAndWalletId(user.Id, wallet.Id);
            var useCase = BuildUseCase(out _, out _, out var unitOfWork, user, wallet);

            // Act
            Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DomainException>()
                .WithMessage($"Wallet with ID {request.WalletId} does not belong to user with ID {request.UserId}");
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(100)]
        [InlineData(999.99)]
        [InlineData(10000)]
        public async Task Given_DifferentValidAmounts_When_DepositingToWallet_Then_ShouldIncreaseBalanceCorrectly(decimal amount)
        {
            // Arrange
            var user = UserBuilder.Build();
            var wallet = WalletBuilder.Build(user.Id);
            var initialBalance = wallet.Balance;

            var request = new DepositToWalletRequest
            {
                UserId = user.Id,
                WalletId = wallet.Id,
                Amount = amount
            };

            var useCase = BuildUseCase(out _, out _, out _, user, wallet);

            // Act
            var result = await useCase.Handle(request, CancellationToken.None);

            // Assert
            result.NewBalance.Should().Be(initialBalance + amount);
            result.DepositedAmount.Should().Be(amount);
        }
    }
}
