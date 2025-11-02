using FCG.Application.UseCases.Admin.CreateUser;
using FCG.CommomTestsUtilities.Builders.Inputs.Admin.CreateUser;
using FCG.Messages;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Admin.CreateUser
{
    public class CreateUserByAdminInputValidatorTest
    {
        private readonly CreateUserByAdminInputValidator _validator;

        public CreateUserByAdminInputValidatorTest()
        {
            _validator = new CreateUserByAdminInputValidator();
        }

        [Fact]
        public void Given_ValidRequest_When_Validating_Then_ShouldNotHaveErrors()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.Build();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Given_ValidRequestWithAdminRole_When_Validating_Then_ShouldNotHaveErrors()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.BuildWithAdminRole();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Given_EmptyName_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.BuildWithEmptyName();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.ErrorMessage == ResourceMessages.NameRequired);
        }

        [Fact]
        public void Given_LongName_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.BuildWithLongName();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Name");
            result.Errors.Should().Contain(e => e.ErrorMessage == ResourceMessages.NameIsLong);
        }

        [Fact]
        public void Given_EmptyEmail_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.BuildWithEmptyEmail();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Email");
            result.Errors.Should().Contain(e => e.ErrorMessage == ResourceMessages.LoginEmalRequired);
        }

        [Fact]
        public void Given_InvalidEmailFormat_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.BuildWithInvalidEmail();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Email");
            result.Errors.Should().Contain(e => e.ErrorMessage == ResourceMessages.LoginInvalidEmailFormat);
        }

        [Fact]
        public void Given_EmptyPassword_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.BuildWithEmptyPassword();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
            result.Errors.Should().Contain(e => e.ErrorMessage == ResourceMessages.LoginPasswordRequired);
        }

        [Fact]
        public void Given_WeakPassword_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.BuildWithWeakPassword();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
            result.Errors.Should().Contain(e => e.ErrorMessage == ResourceMessages.Password);
        }

        [Fact]
        public void Given_LongPassword_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.BuildWithLongPassword();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
            result.Errors.Should().Contain(e => e.ErrorMessage == ResourceMessages.LongPassword);
        }

        [Fact]
        public void Given_InvalidRole_When_Validating_Then_ShouldHaveError()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.BuildWithInvalidRole();

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Role");
            result.Errors.Should().Contain(e => e.ErrorMessage == "Invalid role specified.");
        }
    }
}
