using FCG.Application.UseCases.Promotions.Create;
using FCG.CommomTestsUtilities.Builders.Inputs.Promotions.Create;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Promotions.Create
{
    public class CreatePromotionInputValidatorTests
    {
        private readonly CreatePromotionInputValidator _validator;

        public CreatePromotionInputValidatorTests()
        {
            _validator = new CreatePromotionInputValidator();
        }

        [Fact]
        public void Given_ValidCreatePromotionInput_When_Validate_Then_ShouldNotHaveValidationErrors()
        {
            var input = CreatePromotionInputBuilder.Build();

            var result = _validator.Validate(input);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Given_CreatePromotionInputWithEmptyGameId_When_Validate_Then_ShouldHaveValidationErrorForGameId()
        {
            var input = CreatePromotionInputBuilder.BuildWithEmptyGameId();

            var result = _validator.Validate(input);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Game ID is required.");
        }

        [Fact]
        public void Given_CreatePromotionInputWithNegativeDiscount_When_Validate_Then_ShouldHaveValidationErrorForDiscount()
        {
            var input = CreatePromotionInputBuilder.BuildWithInvalidDiscount(-10);

            var result = _validator.Validate(input);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Discount percentage must be greater than or equal to 0.");
        }

        [Fact]
        public void Given_CreatePromotionInputWithDiscountOver100_When_Validate_Then_ShouldHaveValidationErrorForDiscount()
        {
            var input = CreatePromotionInputBuilder.BuildWithInvalidDiscount(150);

            var result = _validator.Validate(input);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Discount percentage must be less than or equal to 100.");
        }

        [Fact]
        public void Given_CreatePromotionInputWithPastStartDate_When_Validate_Then_ShouldHaveValidationErrorForStartDate()
        {
            var input = CreatePromotionInputBuilder.BuildWithPastStartDate();

            var result = _validator.Validate(input);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "Start date cannot be in the past.");
        }

        [Fact]
        public void Given_CreatePromotionInputWithEndDateBeforeStartDate_When_Validate_Then_ShouldHaveValidationErrorForEndDate()
        {
            var input = CreatePromotionInputBuilder.BuildWithEndDateBeforeStartDate();

            var result = _validator.Validate(input);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == "End date must be after the start date.");
        }

        [Fact]
        public void Given_CreatePromotionInputWithValidDiscountBoundaries_When_Validate_Then_ShouldNotHaveValidationErrors()
        {
            var inputWithZeroDiscount = CreatePromotionInputBuilder.BuildWithInvalidDiscount(0);
            var inputWith100Discount = CreatePromotionInputBuilder.BuildWithInvalidDiscount(100);

            var resultZero = _validator.Validate(inputWithZeroDiscount);
            var result100 = _validator.Validate(inputWith100Discount);

            resultZero.IsValid.Should().BeTrue();
            result100.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Given_CreatePromotionInputWithTodayStartDate_When_Validate_Then_ShouldNotHaveValidationErrors()
        {
            var input = CreatePromotionInputBuilder.BuildWithDates(DateTime.UtcNow, DateTime.UtcNow.AddDays(10));

            var result = _validator.Validate(input);

            result.IsValid.Should().BeTrue();
        }
    }
}
