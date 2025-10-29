using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FCG.Messages;
using FluentAssertions;

namespace FCG.UnitTests.Domain.ValueObjects
{
    public class PasswordTests
    {
        [Fact]
        public void Given_ValidPassword_When_Create_Then_ShouldCreatePassword()
        {
            // Arrange
            var validPassword = "Password@123";

            // Act
            var password = Password.Create(validPassword);

            // Assert
            password.Value.Should().Be(validPassword);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Given_NullOrEmptyPassword_When_Create_Then_ShouldThrowDomainException(string? invalidPassword)
        {
            // Arrange
            var act = () => Password.Create(invalidPassword!);

            // Act & Assert
            act.Should().Throw<DomainException>()
                .WithMessage(ResourceMessages.PasswordCannotBeNullOrEmpty);
        }

        [Fact]
        public void Given_ShortPassword_When_Create_Then_ShouldThrowDomainException()
        {
            // Arrange
            var shortPassword = "Pass@1";

            // Act
            var act = () => Password.Create(shortPassword);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(ResourceMessages.PasswordMinimumLength);
        }

        [Fact]
        public void Given_PasswordWithoutLetter_When_Create_Then_ShouldThrowDomainException()
        {
            // Arrange
            var passwordWithoutLetter = "12345678@";

            // Act
            var act = () => Password.Create(passwordWithoutLetter);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(ResourceMessages.PasswordMustContainLetter);
        }

        [Fact]
        public void Given_PasswordWithoutDigit_When_Create_Then_ShouldThrowDomainException()
        {
            // Arrange
            var passwordWithoutDigit = "Password@";

            // Act
            var act = () => Password.Create(passwordWithoutDigit);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(ResourceMessages.PasswordMustContainNumber);
        }

        [Fact]
        public void Given_PasswordWithoutSpecialCharacter_When_Create_Then_ShouldThrowDomainException()
        {
            // Arrange
            var passwordWithoutSpecial = "Password123";

            // Act
            var act = () => Password.Create(passwordWithoutSpecial);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(ResourceMessages.PasswordMustContainSpecialCharacter);
        }

        [Fact]
        public void Given_ValidHash_When_CreateFromHash_Then_ShouldCreatePassword()
        {
            // Arrange
            var hash = "hashed_password_value";

            // Act
            var password = Password.CreateFromHash(hash);

            // Assert
            password.Value.Should().Be(hash);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Given_NullOrEmptyHash_When_CreateFromHash_Then_ShouldThrowArgumentNullException(string? invalidHash)
        {
            // Arrange
            var act = () => Password.CreateFromHash(invalidHash!);

            // Act & Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("hashValue")
                .WithMessage($"*{ResourceMessages.StoredHashCannotBeNullOrEmpty}*");
        }

        [Fact]
        public void Given_Password_When_ImplicitOperatorCalled_Then_ShouldReturnValue()
        {
            // Arrange
            var password = Password.Create("Password@123");

            // Act
            string result = password;

            // Assert
            result.Should().Be(password.Value);
        }

        [Fact]
        public void Given_Password_When_ToStringCalled_Then_ShouldReturnValue()
        {
            // Arrange
            var passwordValue = "Password@123";
            var password = Password.Create(passwordValue);

            // Act
            var result = password.ToString();

            // Assert
            result.Should().Be(passwordValue);
        }
    }
}