using FCG.Application.UseCases.Games.Register;
using FCG.CommomTestsUtilities.Builders.Inputs.Games.Register;
using FCG.Messages;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Games.Register
{
    public class RegisterGameInputValidatorTest
    {
        [Fact]
        public void Given_ValidRegisterGameInput_When_Validate_Then_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var input = RegisterGameInputBuilder.Build();
            var validator = new RegisterGameInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Given_RegisterGameInputWithEmptyName_When_Validate_Then_ShouldHaveValidationErrorForName()
        {
            // Arrange
            var input = RegisterGameInputBuilder.BuildWithEmptyName();
            var validator = new RegisterGameInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == ResourceMessages.GameNameIsRequired);
        }

        [Fact]
        public void Given_RegisterGameInputWithLongName_When_Validate_Then_ShouldHaveValidationErrorForNameLength()
        {
            // Arrange
            var input = RegisterGameInputBuilder.BuildWithLongName();
            var validator = new RegisterGameInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == ResourceMessages.GameNameMaxLength);
        }

        [Fact]
        public void Given_RegisterGameInputWithEmptyDescription_When_Validate_Then_ShouldHaveValidationErrorForDescription()
        {
            // Arrange
            var input = RegisterGameInputBuilder.BuildWithEmptyDescription();
            var validator = new RegisterGameInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == ResourceMessages.GameDescriptionIsRequired);
        }

        [Fact]
        public void Given_RegisterGameInputWithLongDescription_When_Validate_Then_ShouldHaveValidationErrorForDescriptionLength()
        {
            // Arrange
            var input = RegisterGameInputBuilder.BuildWithLongDescription();
            var validator = new RegisterGameInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == ResourceMessages.GameDescriptionMaxLength);
        }

        [Fact]
        public void Given_RegisterGameInputWithZeroPrice_When_Validate_Then_ShouldHaveValidationErrorForPrice()
        {
            // Arrange
            var input = RegisterGameInputBuilder.BuildWithZeroPrice();
            var validator = new RegisterGameInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == ResourceMessages.GamePriceMustBeGreaterThanZero);
        }

        [Fact]
        public void Given_RegisterGameInputWithNegativePrice_When_Validate_Then_ShouldHaveValidationErrorForPrice()
        {
            // Arrange
            var input = RegisterGameInputBuilder.BuildWithNegativePrice();
            var validator = new RegisterGameInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == ResourceMessages.GamePriceMustBeGreaterThanZero);
        }

        [Fact]
        public void Given_RegisterGameInputWithEmptyCategory_When_Validate_Then_ShouldHaveValidationErrorForCategory()
        {
            // Arrange
            var input = RegisterGameInputBuilder.BuildWithEmptyCategory();
            var validator = new RegisterGameInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == ResourceMessages.GameCategoryIsRequired);
        }

        [Fact]
        public void Given_RegisterGameInputWithLongCategory_When_Validate_Then_ShouldHaveValidationErrorForCategoryLength()
        {
            // Arrange
            var input = RegisterGameInputBuilder.BuildWithLongCategory();
            var validator = new RegisterGameInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.ErrorMessage == ResourceMessages.GameCategoryMaxLength);
        }
    }
}