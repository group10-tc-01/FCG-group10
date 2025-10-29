using FCG.Application.UseCases.Users.Register;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Users.Register
{
    public class RegisterUserInputValidatorTests
    {
        private readonly RegisterUserInputValidator _validator;

        public RegisterUserInputValidatorTests()
        {
            _validator = new RegisterUserInputValidator();
        }

        [Fact]
        public void Given_ValidInput_When_Validate_Then_ShouldPassValidation()
        {
            // Arrange
            var input = new RegisterUserRequest
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "Password@123"
            };

            // Act
            var result = _validator.Validate(input);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Given_InvalidName_When_Validate_Then_ShouldFailValidation(string? name)
        {
            // Arrange
            var input = new RegisterUserRequest
            {
                Name = name!,
                Email = "test@example.com",
                Password = "Password@123"
            };

            // Act
            var result = _validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterUserRequest.Name));
        }

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("@example.com")]
        [InlineData("test@")]
        public void Given_InvalidEmail_When_Validate_Then_ShouldFailValidation(string email)
        {
            // Arrange
            var input = new RegisterUserRequest
            {
                Name = "Test User",
                Email = email,
                Password = "Password@123"
            };

            // Act
            var result = _validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterUserRequest.Email));
        }

        [Theory]
        [InlineData("short")]
        [InlineData("password")]
        [InlineData("PASSWORD123")]
        [InlineData("password123")]
        public void Given_InvalidPassword_When_Validate_Then_ShouldFailValidation(string password)
        {
            // Arrange
            var input = new RegisterUserRequest
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = password
            };

            // Act
            var result = _validator.Validate(input);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(RegisterUserRequest.Password));
        }
    }
}