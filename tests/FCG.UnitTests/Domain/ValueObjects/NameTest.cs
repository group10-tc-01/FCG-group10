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
            // Arrange
            string validName = "FIFA";

            // Act
            var name = Name.Create(validName);

            // Assert
            name.Should().NotBeNull();
            name.Value.Should().Be(validName);
        }

        [Fact]
        public void Given_ShortName_When_CreateName_Then_ShouldThrowArgumentException()
        {
            // Arrange
            string shortName = "A"; // Menos de 2 caracteres

            // Act
            Action act = () => Name.Create(shortName);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Name must be at least 2 characters long.");
        }

        [Fact]
        public void Given_NullName_When_CreateName_Then_ShouldThrowArgumentException()
        {
            // Arrange
            string nullName = null;

            // Act
            Action act = () => Name.Create(nullName);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Name cannot be null or empty.");
        }

        [Fact]
        public void Given_EmptyName_When_CreateName_Then_ShouldThrowArgumentException()
        {
            // Arrange
            string emptyName = string.Empty;

            // Act
            Action act = () => Name.Create(emptyName);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Name cannot be null or empty.");
        }

        // 🚀 TESTES PARA COMPLETAR OS 25% RESTANTES:

        [Fact]
        public void Given_WhitespaceOnlyName_When_CreateName_Then_ShouldThrowArgumentException()
        {
            // Arrange
            string whitespaceName = "   "; // Só espaços

            // Act
            Action act = () => Name.Create(whitespaceName);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Name cannot be null or empty.");
        }

        [Fact]
        public void Given_ExactlyTwoCharactersName_When_CreateName_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            string minValidName = "AB"; // Exatamente 2 caracteres

            // Act
            var name = Name.Create(minValidName);

            // Assert
            name.Should().NotBeNull();
            name.Value.Should().Be(minValidName);
        }

        [Fact]
        public void Given_VeryLongName_When_CreateName_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            string longName = new string('A', 100); // Nome muito longo

            // Act
            var name = Name.Create(longName);

            // Assert
            name.Should().NotBeNull();
            name.Value.Should().Be(longName);
        }

        [Fact]
        public void Given_NameWithSpecialCharacters_When_CreateName_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            string nameWithSpecialChars = "Call of Duty: Modern Warfare II";

            // Act
            var name = Name.Create(nameWithSpecialChars);

            // Assert
            name.Should().NotBeNull();
            name.Value.Should().Be(nameWithSpecialChars);
        }

        [Fact]
        public void Given_NameWithNumbers_When_CreateName_Then_ShouldCreateSuccessfully()
        {
            // Arrange
            string nameWithNumbers = "FIFA 2024";

            // Act
            var name = Name.Create(nameWithNumbers);

            // Assert
            name.Should().NotBeNull();
            name.Value.Should().Be(nameWithNumbers);
        }

        [Fact]
        public void Given_TwoNamesWithSameValue_When_Compare_Then_ShouldBeEqual()
        {
            // Arrange
            var name1 = Name.Create("Minecraft");
            var name2 = Name.Create("Minecraft");

            // Act & Assert
            name1.Should().Be(name2);
            name1.GetHashCode().Should().Be(name2.GetHashCode());
        }

        [Fact]
        public void Given_TwoNamesWithDifferentValues_When_Compare_Then_ShouldNotBeEqual()
        {
            // Arrange
            var name1 = Name.Create("Minecraft");
            var name2 = Name.Create("Terraria");

            // Act & Assert
            name1.Should().NotBe(name2);
        }

        [Fact]
        public void Given_NameObject_When_ConvertToString_Then_ShouldReturnValue()
        {
            // Arrange
            var name = Name.Create("Grand Theft Auto V");

            // Act
            string stringValue = name.ToString();

            // Assert
            stringValue.Should().Be("Grand Theft Auto V");
        }

        [Fact]
        public void Given_StringValue_When_ImplicitConversionToName_Then_ShouldCreateName()
        {
            // Arrange
            string stringValue = "TheBoy";

            // Act
            Name name = stringValue; // Conversão implícita

            // Assert
            name.Should().NotBeNull();
            name.Value.Should().Be(stringValue);
        }

        [Fact]
        public void Given_NameObject_When_ImplicitConversionToString_Then_ShouldReturnValue()
        {
            // Arrange
            var name = Name.Create("Cyberpunk");

            // Act
            string stringValue = name; // Conversão implícita

            // Assert
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
