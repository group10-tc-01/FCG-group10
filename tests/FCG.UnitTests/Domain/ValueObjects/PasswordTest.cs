using FCG.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.UnitTests.Domain.ValueObjects
{
    public class PasswordTest
    {

        [Fact]
        public void Given_ValidPassword_When_CreatePassword_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            string validPassword = "MySecure123!";

            // Act
            var password = Password.Create(validPassword);

            // Assert
            password.Should().NotBeNull();
            password.Value.Should().Be(validPassword);
        }

        [Fact]
        public void Given_MinimumValidPassword_When_CreatePassword_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            string minPassword = "Abcd123!"; // 8 characters

            // Act
            var password = Password.Create(minPassword);

            // Assert
            password.Value.Should().Be(minPassword);
        }

        [Fact]
        public void Given_NullPassword_When_CreatePassword_Then_ShouldThrowArgumentException()
        {
            // Arrange
            string nullPassword = null;

            // Act
            Action act = () => Password.Create(nullPassword);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Password cannot be null or empty.");
        }

        [Fact]
        public void Given_EmptyPassword_When_CreatePassword_Then_ShouldThrowArgumentException()
        {
            // Arrange
            string emptyPassword = string.Empty;

            // Act
            Action act = () => Password.Create(emptyPassword);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Password cannot be null or empty.");
        }

        [Fact]
        public void Given_WhitespacePassword_When_CreatePassword_Then_ShouldThrowArgumentException()
        {
            // Arrange
            string whitespacePassword = "        ";

            // Act
            Action act = () => Password.Create(whitespacePassword);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Password cannot be null or empty.");
        }

        [Fact]
        public void Given_ShortPassword_When_CreatePassword_Then_ShouldThrowArgumentException()
        {
            // Arrange
            string shortPassword = "Abc1!"; // Menos de 8 caracteres

            // Act
            Action act = () => Password.Create(shortPassword);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Password must be at least 8 characters long.");
        }

        [Fact]
        public void Given_PasswordWithoutLetter_When_CreatePassword_Then_ShouldThrowArgumentException()
        {
            // Arrange
            string passwordWithoutLetter = "12345678!";

            // Act
            Action act = () => Password.Create(passwordWithoutLetter);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Password must contain at least one letter.");
        }

        [Fact]
        public void Given_PasswordWithoutDigit_When_CreatePassword_Then_ShouldThrowArgumentException()
        {
            // Arrange
            string passwordWithoutDigit = "Password!";

            // Act
            Action act = () => Password.Create(passwordWithoutDigit);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Password must contain at least one number.");
        }

        [Fact]
        public void Given_PasswordWithoutSpecialCharacter_When_CreatePassword_Then_ShouldThrowArgumentException()
        {
            // Arrange
            string passwordWithoutSpecial = "Password123";

            // Act
            Action act = () => Password.Create(passwordWithoutSpecial);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Password must contain at least one special character.");
        }

        [Fact]
        public void Given_LongValidPassword_When_CreatePassword_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            string longPassword = "ThisIsAVeryLongAndSecurePassword123!@#";

            // Act
            var password = Password.Create(longPassword);

            // Assert
            password.Value.Should().Be(longPassword);
        }

        [Fact]
        public void Given_PasswordObject_When_ImplicitConvertToString_Then_ShouldReturnValue()
        {
            // Arrange
            var password = Password.Create("Convert123!");

            // Act
            string value = password;

            // Assert
            value.Should().Be("Convert123!");
        }

        [Fact]
        public void Given_StringValue_When_ImplicitConvertToPassword_Then_ShouldCreatePassword()
        {
            // Arrange
            string value = "StringConvert123!";

            // Act
            Password password = value;

            // Assert
            password.Value.Should().Be("StringConvert123!");
        }

        [Fact]
        public void Given_PasswordObject_When_CallToString_Then_ShouldReturnValue()
        {
            // Arrange
            var password = Password.Create("ToString123!");

            // Act
            string result = password.ToString();

            // Assert
            result.Should().Be("ToString123!");
        }

        [Fact]
        public void Given_TwoPasswordsWithSameValue_When_Compare_Then_ShouldBeEqual()
        {
            // Arrange
            var password1 = Password.Create("SamePass123!");
            var password2 = Password.Create("SamePass123!");

            // Act & Assert
            password1.Should().Be(password2);
        }

        [Fact]
        public void Given_TwoPasswordsWithDifferentValues_When_Compare_Then_ShouldNotBeEqual()
        {
            // Arrange
            var password1 = Password.Create("FirstPass123!");
            var password2 = Password.Create("SecondPass123!");

            // Act & Assert
            password1.Should().NotBe(password2);
        }
    }
}