using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FluentAssertions;

namespace FCG.UnitTests.Domain.Entities
{
    public class WalletTests
    {
        [Fact]
        public void Given_ValidUserId_When_Create_Then_ShouldInstantiateWalletWithInitialBalance()
        {
            // Arrange
            var user = UserBuilder.Build();
            var initialBalance = 10;

            // Act
            var wallet = Wallet.Create(user.Id);

            // Assert
            wallet.Should().NotBeNull();
            wallet.Id.Should().NotBe(Guid.Empty);
            wallet.UserId.Should().Be(user.Id);
            wallet.Balance.Should().Be(initialBalance);
        }

        [Fact]
        public void Given_PositiveAmount_When_Deposit_Then_ShouldIncreaseBalance()
        {
            // Arrange
            var wallet = WalletBuilder.Build();
            var initialBalance = wallet.Balance;
            var depositAmount = 50.00m;

            // Act
            wallet.Deposit(depositAmount);

            // Assert
            wallet.Balance.Should().Be(initialBalance + depositAmount);
        }

        [Fact]
        public void Given_ZeroAmount_When_Deposit_Then_ShouldThrowDomainException()
        {
            // Arrange
            var wallet = WalletBuilder.Build();

            // Act
            Action act = () => wallet.Deposit(0);

            // Assert
            act.Should().Throw<FCG.Domain.Exceptions.DomainException>()
                .WithMessage("Deposit amount must be greater than zero.");
        }

        [Fact]
        public void Given_NegativeAmount_When_Deposit_Then_ShouldThrowDomainException()
        {
            // Arrange
            var wallet = WalletBuilder.Build();

            // Act
            Action act = () => wallet.Deposit(-100.00m);

            // Assert
            act.Should().Throw<FCG.Domain.Exceptions.DomainException>()
                .WithMessage("Deposit amount must be greater than zero.");
        }

        [Fact]
        public void Given_MultipleDeposits_When_Deposit_Then_ShouldAccumulateBalance()
        {
            // Arrange
            var wallet = WalletBuilder.Build();
            var initialBalance = wallet.Balance;

            // Act
            wallet.Deposit(100.00m);
            wallet.Deposit(50.00m);
            wallet.Deposit(25.50m);

            // Assert
            wallet.Balance.Should().Be(initialBalance + 175.50m);
        }
    }
}
