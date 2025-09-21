using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FluentAssertions;

namespace FCG.UnitTests.Domain.Entities
{
    public class WalletTests
    {
        [Fact]
        public void Given_ValidUserId_When_CreateWallet_Then_WalletIsInstantiatedCorrectlyWithZeroBalance()
        {
            var userId = Guid.NewGuid();

            var wallet = Wallet.Create(userId, 1);

            wallet.Should().NotBeNull();
            wallet.Id.Should().NotBe(Guid.Empty);
            wallet.UserId.Should().Be(userId);
            wallet.Balance.Should().Be(1);
        }
    }
}