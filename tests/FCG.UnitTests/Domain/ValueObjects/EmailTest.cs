using FCG.Domain.ValueObjects;
using FluentAssertions;

namespace FCG.UnitTests.Domain.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void Given_ValidEmail_When_CreateEmail_Then_ShouldCreateSuccessfully()
        {

            string validEmail = "user@example.com";

            var email = Email.Create(validEmail);

            email.Should().NotBeNull();
            email.Value.Should().Be(validEmail);
        }

        [Fact]
        public void Given_EmailWithSubdomain_When_CreateEmail_Then_ShouldCreateSuccessfully()
        {
            string emailWithSubdomain = "user@mail.example.com";

            var email = Email.Create(emailWithSubdomain);

            email.Value.Should().Be(emailWithSubdomain);
        }

        [Fact]
        public void Given_EmailWithPlusSign_When_CreateEmail_Then_ShouldCreateSuccessfully()
        {
            string emailWithPlus = "user+tag@example.com";

            var email = Email.Create(emailWithPlus);

            email.Value.Should().Be(emailWithPlus);
        }

        [Fact]
        public void Given_NullEmail_When_CreateEmail_Then_ShouldThrowArgumentException()
        {

            string nullEmail = null;

            Action act = () => Email.Create(nullEmail);

            act.Should().Throw<ArgumentException>()
               .WithMessage("Email cannot be null or empty.");
        }

        [Fact]
        public void Given_EmptyEmail_When_CreateEmail_Then_ShouldThrowArgumentException()
        {
            string emptyEmail = string.Empty;

            Action act = () => Email.Create(emptyEmail);

            act.Should().Throw<ArgumentException>()
               .WithMessage("Email cannot be null or empty.");
        }

        [Fact]
        public void Given_WhitespaceEmail_When_CreateEmail_Then_ShouldThrowArgumentException()
        {
            string whitespaceEmail = "   ";

            Action act = () => Email.Create(whitespaceEmail);


            act.Should().Throw<ArgumentException>()
               .WithMessage("Email cannot be null or empty.");
        }

        [Fact]
        public void Given_InvalidEmailFormat_When_CreateEmail_Then_ShouldThrowArgumentException()
        {

            string invalidEmail = "invalid-email";


            Action act = () => Email.Create(invalidEmail);

            act.Should().Throw<ArgumentException>()
               .WithMessage("Invalid email format.");
        }

        [Fact]
        public void Given_EmailWithoutAtSymbol_When_CreateEmail_Then_ShouldThrowArgumentException()
        {
            string emailWithoutAt = "userexample.com";


            Action act = () => Email.Create(emailWithoutAt);


            act.Should().Throw<ArgumentException>()
               .WithMessage("Invalid email format.");
        }

        [Fact]
        public void Given_EmailLongerThan255Characters_When_CreateEmail_Then_ShouldThrowArgumentException()
        {

            string longEmail = new string('a', 256) + "@example.com";

            Action act = () => Email.Create(longEmail);


            act.Should().Throw<ArgumentException>()
               .WithMessage("Email cannot be longer than 255 characters.");
        }

        [Fact]
        public void Given_EmailObject_When_ImplicitConvertToString_Then_ShouldReturnValue()
        {
            var email = Email.Create("test@example.com");

            string value = email;


            value.Should().Be("test@example.com");
        }

        [Fact]
        public void Given_StringValue_When_ImplicitConvertToEmail_Then_ShouldCreateEmail()
        {
            string value = "convert@example.com";


            Email email = value;


            email.Value.Should().Be("convert@example.com");
        }

        [Fact]
        public void Given_EmailObject_When_CallToString_Then_ShouldReturnValue()
        {

            var email = Email.Create("tostring@example.com");


            string result = email.ToString();


            result.Should().Be("tostring@example.com");
        }

        [Fact]
        public void Given_TwoEmailsWithSameValue_When_Compare_Then_ShouldBeEqual()
        {

            var email1 = Email.Create("same@example.com");
            var email2 = Email.Create("same@example.com");


            email1.Should().Be(email2);
        }

        [Fact]
        public void Given_TwoEmailsWithDifferentValues_When_Compare_Then_ShouldNotBeEqual()
        {

            var email1 = Email.Create("first@example.com");
            var email2 = Email.Create("second@example.com");

            email1.Should().NotBe(email2);
        }
    }
}
