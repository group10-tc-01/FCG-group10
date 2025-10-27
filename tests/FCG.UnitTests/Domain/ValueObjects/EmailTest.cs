using FCG.Domain.Exceptions;
using FCG.Domain.ValueObjects;
using FCG.Messages;
using FluentAssertions;

namespace FCG.UnitTests.Domain.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void Given_ValidEmail_When_CreateEmail_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            string validEmail = "user@example.com";

            // Act
            var email = Email.Create(validEmail);

            // Assert
            email.Should().NotBeNull();
            email.Value.Should().Be(validEmail);
        }

        [Fact]
        public void Given_EmailWithSubdomain_When_CreateEmail_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            string emailWithSubdomain = "user@mail.example.com";

            // Act
            var email = Email.Create(emailWithSubdomain);

            // Assert
            email.Value.Should().Be(emailWithSubdomain);
        }

        [Fact]
        public void Given_EmailWithPlusSign_When_CreateEmail_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            string emailWithPlus = "user+tag@example.com";

            // Act
            var email = Email.Create(emailWithPlus);

            // Assert
            email.Value.Should().Be(emailWithPlus);
        }

        [Fact]
        public void Given_NullEmail_When_CreateEmail_Then_ShouldThrowDomainException()
        {
            // Arrange
            string nullEmail = null;

            // Act
            Action act = () => Email.Create(nullEmail);

            // Assert
            act.Should().Throw<DomainException>().WithMessage(ResourceMessages.EmailCannotBeNullOrEmpty);
        }

        [Fact]
        public void Given_EmptyEmail_When_CreateEmail_Then_ShouldThrowDomainException()
        {
            // Arrange
            string emptyEmail = string.Empty;

            // Act
            Action act = () => Email.Create(emptyEmail);

            // Assert
            act.Should().Throw<DomainException>().WithMessage(ResourceMessages.EmailCannotBeNullOrEmpty);
        }

        [Fact]
        public void Given_WhitespaceEmail_When_CreateEmail_Then_ShouldThrowDomainException()
        {
            // Arrange
            string whitespaceEmail = "   ";

            // Act
            Action act = () => Email.Create(whitespaceEmail);

            // Assert
            act.Should().Throw<DomainException>().WithMessage(ResourceMessages.EmailCannotBeNullOrEmpty);
        }

        [Fact]
        public void Given_InvalidEmailFormat_When_CreateEmail_Then_ShouldThrowDomainException()
        {
            // Arrange
            string invalidEmail = "invalid-email";

            // Act
            Action act = () => Email.Create(invalidEmail);

            // Assert
            act.Should().Throw<DomainException>().WithMessage(ResourceMessages.InvalidEmailFormat);
        }

        [Fact]
        public void Given_EmailWithoutAtSymbol_When_CreateEmail_Then_ShouldThrowDomainException()
        {
            // Arrange
            string emailWithoutAt = "userexample.com";

            // Act
            Action act = () => Email.Create(emailWithoutAt);

            // Assert
            act.Should().Throw<DomainException>().WithMessage(ResourceMessages.InvalidEmailFormat);
        }

        [Fact]
        public void Given_EmailLongerThan255Characters_When_CreateEmail_Then_ShouldThrowDomainException()
        {
            // Arrange
            string longEmail = new string('a', 256) + "@example.com";

            // Act
            Action act = () => Email.Create(longEmail);

            // Assert
            act.Should().Throw<DomainException>().WithMessage(ResourceMessages.EmailCannotExceed255Characters);
        }

        [Fact]
        public void Given_EmailObject_When_ImplicitConvertToString_Then_ShouldReturnValue()
        {
            // Arrange
            var email = Email.Create("test@example.com");

            // Act
            string value = email;

            // Assert
            value.Should().Be("test@example.com");
        }

        [Fact]
        public void Given_StringValue_When_ImplicitConvertToEmail_Then_ShouldCreateEmail()
        {
            // Arrange
            string value = "convert@example.com";

            // Act
            Email email = value;

            // Assert
            email.Value.Should().Be("convert@example.com");
        }

        [Fact]
        public void Given_EmailObject_When_CallToString_Then_ShouldReturnValue()
        {
            // Arrange
            var email = Email.Create("tostring@example.com");

            // Act
            string result = email.ToString();

            // Assert
            result.Should().Be("tostring@example.com");
        }
    }
}
