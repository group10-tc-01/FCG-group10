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

        [Fact(DisplayName = "Deve validar com sucesso quando todos os campos são válidos")]
        public void Validate_GivenValidInput_ShouldPassValidation()
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
        public void Validate_GivenInvalidName_ShouldFailValidation(string? name)
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
        public void Validate_GivenInvalidEmail_ShouldFailValidation(string email)
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
        public void Validate_GivenInvalidPassword_ShouldFailValidation(string password)
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