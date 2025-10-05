using FCG.Application.UseCases.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Login;
using FCG.Messages;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Authentication.Login
{
    public class LoginInputValidatorTest
    {
        [Fact]
        public void Given_ValidLoginInput_When_Validate_Then_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var input = LoginInputBuilder.Build();

            var validator = new LoginInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Given_LoginInputWithEmptyEmail_When_Validate_Then_ShouldHaveValidationErrorForEmail()
        {
            // Arrange
            var input = LoginInputBuilder.BuildWithEmptyEmail();
            var validator = new LoginInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Contain(e => e.ErrorMessage == ResourceMessages.LoginEmalRequired);
            result.Errors.Should().Contain(e => e.ErrorMessage == ResourceMessages.LoginInvalidEmailFormat);
        }

        [Fact]
        public void Given_LoginInputWithInvalidEmail_When_Validate_Then_ShouldHaveValidationErrorForEmailFormat()
        {
            // Arrange
            var input = LoginInputBuilder.BuildWithInvalidEmail();
            var validator = new LoginInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(ResourceMessages.LoginInvalidEmailFormat);
        }

        [Fact]
        public void Given_LoginInputWithEmptyPassword_When_Validate_Then_ShouldHaveValidationErrorForPassword()
        {
            // Arrange
            var input = LoginInputBuilder.BuildWithEmptyPassword();
            var validator = new LoginInputValidator();

            // Act
            var result = validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(ResourceMessages.LoginPasswordRequired);
        }
    }
}
