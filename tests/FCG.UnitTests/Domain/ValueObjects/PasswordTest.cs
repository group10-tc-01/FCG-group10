using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FluentAssertions;

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
            string minPassword = "Abcd123!";

            // Act
            var password = Password.Create(minPassword);

            // Assert
            password.Value.Should().Be(minPassword);
        }

        [Fact]
        public void Given_NullPassword_When_CreatePassword_Then_ShouldThrowDomainException()
        {

            string nullPassword = null;

            Action act = () => Password.Create(nullPassword);

            act.Should().Throw<DomainException>().WithMessage("Password cannot be null or empty.");
        }

        [Fact]
        public void Given_EmptyPassword_When_CreatePassword_Then_ShouldThrowDomainException()
        {
            string emptyPassword = string.Empty;

            Action act = () => Password.Create(emptyPassword);

            act.Should().Throw<DomainException>().WithMessage("Password cannot be null or empty.");
        }

        [Fact]
        public void Given_WhitespacePassword_When_CreatePassword_Then_ShouldThrowDomainException()
        {
            string whitespacePassword = "        ";

            Action act = () => Password.Create(whitespacePassword);

            act.Should().Throw<DomainException>().WithMessage("Password cannot be null or empty.");
        }

        [Fact]
        public void Given_ShortPassword_When_CreatePassword_Then_ShouldThrowDomainException()
        {
            string shortPassword = "Abc1!";

            Action act = () => Password.Create(shortPassword);

            act.Should().Throw<DomainException>().WithMessage("Password must be at least 8 characters long.");
        }

        [Fact]
        public void Given_PasswordWithoutLetter_When_CreatePassword_Then_ShouldThrowDomainException()
        {
            string passwordWithoutLetter = "12345678!";

            Action act = () => Password.Create(passwordWithoutLetter);

            act.Should().Throw<DomainException>().WithMessage("Password must contain at least one letter.");
        }

        [Fact]
        public void Given_PasswordWithoutDigit_When_CreatePassword_Then_ShouldThrowDomainException()
        {
            string passwordWithoutDigit = "Password!";

            Action act = () => Password.Create(passwordWithoutDigit);

            act.Should().Throw<DomainException>().WithMessage("Password must contain at least one number.");
        }

        [Fact]
        public void Given_PasswordWithoutSpecialCharacter_When_CreatePassword_Then_ShouldThrowDomainException()
        {
            string passwordWithoutSpecial = "Password123";

            Action act = () => Password.Create(passwordWithoutSpecial);

            act.Should().Throw<DomainException>().WithMessage("Password must contain at least one special character.");
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
        [Theory]
        [InlineData(null, "Password cannot be null or empty.")]
        [InlineData("1234567", "Password must be at least 8 characters long.")] // Falha 1: Comprimento
        [InlineData("12345678!@#", "Password must contain at least one letter.")] // Falha 2: Falta Letra
        [InlineData("ABCDEFGHI!", "Password must contain at least one number.")] // Falha 3: Falta Dígito
        [InlineData("Abcdefgh123", "Password must contain at least one special character.")] // Falha 4: Falta Especial
        public void PasswordCreate_GivenInvalidRequirements_ShouldThrowDomainException(string? invalidPassword, string expectedMessagePart)
        {
            // ARRANGE / ACT / ASSERT
            var act = () => Password.Create(invalidPassword!);

            act.Should().Throw<DomainException>()
                .WithMessage($"*{expectedMessagePart}*");
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
        public void Given_TwoPasswordsWithDifferentValues_When_Compare_Then_ShouldNotBeEqual()
        {
            var password1 = Password.Create("FirstPass123!");
            var password2 = Password.Create("SecondPass123!");
            password1.Should().NotBe(password2);
        }
    }
}