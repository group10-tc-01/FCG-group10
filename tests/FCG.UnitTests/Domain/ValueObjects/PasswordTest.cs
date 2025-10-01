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
            const string plainTextPassword = "MySecure123!";

            var password = Password.Create(plainTextPassword);

            password.Should().NotBeNull();
            password.Value.Should().NotBe(plainTextPassword, "A senha deve ser hasheada.");

            password.Value.Should().MatchRegex(@"^\$2[abxy]\$\d{2}\$.{53}$");

            password.VerifyPassword(plainTextPassword).Should().BeTrue("A verificação com a senha original deve ser True.");

            password.VerifyPassword("WrongPassword!").Should().BeFalse("A verificação com senha errada deve ser False.");
        }

        [Fact]
        public void Given_MinimumValidPassword_When_CreatePassword_Then_ShouldCreateSuccessfully()
        {

            const string plainTextPassword = "Abcd123!";
            var password = Password.Create(plainTextPassword);
            password.VerifyPassword(plainTextPassword).Should().BeTrue();
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
            const string plainTextPassword = "ThisIsAVeryLongAndSecurePassword123!@#$";
            var password = Password.Create(plainTextPassword);
            password.VerifyPassword(plainTextPassword).Should().BeTrue();
        }

        [Fact]
        public void Given_PasswordObject_When_ImplicitConvertToString_Then_ShouldReturnValue()
        {
            const string plainTextPassword = "Convert123!";
            var password = Password.Create(plainTextPassword);

            string value = password;

            value.Should().NotBe(plainTextPassword, "A conversão implícita deve retornar o hash, não a senha clara.");
            value.Should().MatchRegex(@"^\$2[abxy]\$\d{2}\$.{53}$");
        }

        [Fact]
        public void Given_PasswordObject_When_CallToString_Then_ShouldReturnValue()
        {
            const string plainTextPassword = "ToString123!";
            var password = Password.Create(plainTextPassword);

            string result = password.ToString();

            result.Should().NotBe(plainTextPassword, "ToString deve retornar o hash.");
            result.Should().MatchRegex(@"^\$2[abxy]\$\d{2}\$.{53}$");
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