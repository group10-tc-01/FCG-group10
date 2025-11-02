using FCG.Application.UseCases.Admin.DepositToWallet;
using FCG.CommomTestsUtilities.Builders.Inputs.Admin.DepositToWallet;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Admin.DepositToWallet
{
    public class DepositToWalletInputValidatorTest
    {
        private readonly DepositToWalletInputValidator _validator;

        public DepositToWalletInputValidatorTest()
        {
            _validator = new DepositToWalletInputValidator();
        }

        [Fact]
        public void Given_ValidRequest_When_Validating_Then_ShouldNotHaveErrors()
        {
            // Arrange
            var request = DepositToWalletInputBuilder.Build();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Given_EmptyUserId_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = DepositToWalletInputBuilder.BuildWithEmptyUserId();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "UserId");
            result.Errors.Should().Contain(e => e.ErrorMessage == "User ID is required.");
        }

        [Fact]
        public void Given_EmptyWalletId_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = DepositToWalletInputBuilder.BuildWithEmptyWalletId();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "WalletId");
            result.Errors.Should().Contain(e => e.ErrorMessage == "Wallet ID is required.");
        }

        [Fact]
        public void Given_ZeroAmount_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = DepositToWalletInputBuilder.BuildWithZeroAmount();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Amount");
            result.Errors.Should().Contain(e => e.ErrorMessage == "Deposit amount must be greater than zero.");
        }

        [Fact]
        public void Given_NegativeAmount_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = DepositToWalletInputBuilder.BuildWithNegativeAmount();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Amount");
            result.Errors.Should().Contain(e => e.ErrorMessage == "Deposit amount must be greater than zero.");
        }

        [Fact]
        public void Given_ExcessiveAmount_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = DepositToWalletInputBuilder.BuildWithExcessiveAmount();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Amount");
            result.Errors.Should().Contain(e => e.ErrorMessage == "Deposit amount cannot exceed 1,000,000.");
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(100)]
        [InlineData(999999.99)]
        [InlineData(1000000)]
        public void Given_ValidAmounts_When_Validating_Then_ShouldNotHaveErrors(decimal amount)
        {
            // Arrange
            var request = DepositToWalletInputBuilder.BuildWithAmount(amount);

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
