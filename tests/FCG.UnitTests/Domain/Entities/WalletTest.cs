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
    }
}
