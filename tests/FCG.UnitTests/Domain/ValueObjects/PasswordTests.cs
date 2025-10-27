using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FluentAssertions;

namespace FCG.UnitTests.Domain.ValueObjects
{
    public class PasswordTests
    {
        [Fact(DisplayName = "Create deve criar password com senha válida")]
        public void Create_GivenValidPassword_ShouldCreatePassword()
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
        public void Create_GivenNullOrEmptyPassword_ShouldThrowDomainException(string? invalidPassword)
        {
            // Act
            var act = () => Password.Create(invalidPassword!);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(ResourceMessages.PasswordCannotBeNullOrEmpty);
        }

        [Fact(DisplayName = "Create deve lançar exceção quando senha é muito curta")]
        public void Create_GivenShortPassword_ShouldThrowDomainException()
        {
            // Arrange
            var shortPassword = "Pass@1";

            // Act
            var act = () => Password.Create(shortPassword);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(ResourceMessages.PasswordMinimumLength);
        }

        [Fact(DisplayName = "Create deve lançar exceção quando senha não contém letra")]
        public void Create_GivenPasswordWithoutLetter_ShouldThrowDomainException()
        {
            // Arrange
            var passwordWithoutLetter = "12345678@";

            // Act
            var act = () => Password.Create(passwordWithoutLetter);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(ResourceMessages.PasswordMustContainLetter);
        }

        [Fact(DisplayName = "Create deve lançar exceção quando senha não contém dígito")]
        public void Create_GivenPasswordWithoutDigit_ShouldThrowDomainException()
        {
            // Arrange
            var passwordWithoutDigit = "Password@";

            // Act
            var act = () => Password.Create(passwordWithoutDigit);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(ResourceMessages.PasswordMustContainNumber);
        }

        [Fact(DisplayName = "Create deve lançar exceção quando senha não contém caractere especial")]
        public void Create_GivenPasswordWithoutSpecialCharacter_ShouldThrowDomainException()
        {
            // Arrange
            var passwordWithoutSpecial = "Password123";

            // Act
            var act = () => Password.Create(passwordWithoutSpecial);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(ResourceMessages.PasswordMustContainSpecialCharacter);
        }

        [Fact(DisplayName = "CreateFromHash deve criar password com hash válido")]
        public void CreateFromHash_GivenValidHash_ShouldCreatePassword()
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
        public void CreateFromHash_GivenNullOrEmptyHash_ShouldThrowArgumentNullException(string? invalidHash)
        {
            // Act
            var act = () => Password.CreateFromHash(invalidHash!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("hashValue")
                .WithMessage($"*{ResourceMessages.StoredHashCannotBeNullOrEmpty}*");
        }

        [Fact(DisplayName = "Operador implícito string deve retornar Value")]
        public void ImplicitOperator_GivenPassword_ShouldReturnValue()
        {
            // Arrange
            var password = Password.Create("Password@123");

            // Act
            string result = password;

            // Assert
            result.Should().Be(password.Value);
        }

        [Fact(DisplayName = "ToString deve retornar Value")]
        public void ToString_ShouldReturnValue()
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