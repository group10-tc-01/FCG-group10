// FCG.UnitTests/Domain/ValueObjects/NameTests.cs

using FCG.Domain.ValueObjects;
using FluentAssertions;

namespace FCG.UnitTests.Domain.ValueObjects
{
    public class NameTests
    {


        [Fact]
        public void Given_ValidName_When_CreateName_Then_ShouldCreateSuccessfully()
        {

            string validName = "FIFA";


            var name = Name.Create(validName);


            name.Should().NotBeNull();
            name.Value.Should().Be(validName);
        }

        [Fact]
        public void Given_ShortName_When_CreateName_Then_ShouldThrowArgumentException()
        {

            string shortName = "A";

            Action act = () => Name.Create(shortName);


            act.Should().Throw<ArgumentException>()
               .WithMessage("Name must be at least 2 characters long.");
        }

        [Fact]
        public void Given_NullName_When_CreateName_Then_ShouldThrowArgumentException()
        {

            string nullName = null;


            Action act = () => Name.Create(nullName);


            act.Should().Throw<ArgumentException>()
               .WithMessage("Name cannot be null or empty.");
        }

        [Fact]
        public void Given_EmptyName_When_CreateName_Then_ShouldThrowArgumentException()
        {

            string emptyName = string.Empty;


            Action act = () => Name.Create(emptyName);


            act.Should().Throw<ArgumentException>()
               .WithMessage("Name cannot be null or empty.");
        }



        [Fact]
        public void Given_WhitespaceOnlyName_When_CreateName_Then_ShouldThrowArgumentException()
        {

            string whitespaceName = "   ";


            Action act = () => Name.Create(whitespaceName);


            act.Should().Throw<ArgumentException>()
               .WithMessage("Name cannot be null or empty.");
        }

        [Fact]
        public void Given_ExactlyTwoCharactersName_When_CreateName_Then_ShouldCreateSuccessfully()
        {

            string minValidName = "AB";


            var name = Name.Create(minValidName);

            name.Should().NotBeNull();
            name.Value.Should().Be(minValidName);
        }

        [Fact]
        public void Given_VeryLongName_When_CreateName_Then_ShouldCreateSuccessfully()
        {

            string longName = new string('A', 100);


            var name = Name.Create(longName);


            name.Should().NotBeNull();
            name.Value.Should().Be(longName);
        }

        [Fact]
        public void Given_NameWithSpecialCharacters_When_CreateName_Then_ShouldCreateSuccessfully()
        {

            string nameWithSpecialChars = "Call of Duty: Modern Warfare II";


            var name = Name.Create(nameWithSpecialChars);


            name.Should().NotBeNull();
            name.Value.Should().Be(nameWithSpecialChars);
        }

        [Fact]
        public void Given_NameWithNumbers_When_CreateName_Then_ShouldCreateSuccessfully()
        {

            string nameWithNumbers = "FIFA 2024";


            var name = Name.Create(nameWithNumbers);


            name.Should().NotBeNull();
            name.Value.Should().Be(nameWithNumbers);
        }

        [Fact]
        public void Given_TwoNamesWithSameValue_When_Compare_Then_ShouldBeEqual()
        {

            var name1 = Name.Create("Minecraft");
            var name2 = Name.Create("Minecraft");


            name1.Should().Be(name2);
            name1.GetHashCode().Should().Be(name2.GetHashCode());
        }

        [Fact]
        public void Given_TwoNamesWithDifferentValues_When_Compare_Then_ShouldNotBeEqual()
        {

            var name1 = Name.Create("Minecraft");
            var name2 = Name.Create("Terraria");


            name1.Should().NotBe(name2);
        }

        [Fact]
        public void Given_NameObject_When_ConvertToString_Then_ShouldReturnValue()
        {

            var name = Name.Create("Grand Theft Auto V");


            string stringValue = name.ToString();


            stringValue.Should().Be("Grand Theft Auto V");
        }

        [Fact]
        public void Given_StringValue_When_ImplicitConversionToName_Then_ShouldCreateName()
        {

            string stringValue = "TheBoy";


            Name name = stringValue;

            name.Should().NotBeNull();
            name.Value.Should().Be(stringValue);
        }

        [Fact]
        public void Given_NameObject_When_ImplicitConversionToString_Then_ShouldReturnValue()
        {
            var name = Name.Create("Cyberpunk");

            string stringValue = name; // Conversão implícita

            stringValue.Should().Be("Cyberpunk");
        }

        [Fact]
        public void Given_NameWithLeadingAndTrailingSpaces_When_CreateName_Then_ShouldTrimSpaces()
        {

            string nameWithSpaces = "  Assassin";


            var name = Name.Create(nameWithSpaces);


            name.Value.Should().Be("Assassin");
        }
    }
}
