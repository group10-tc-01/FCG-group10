using FCG.Domain;

namespace FCG.UnitTests
{
    public class SonarTest
    {
        [Theory]
        [InlineData("name1")]
        [InlineData("name2")]
        public void Sonar_Should_Have_Correct_Name(string name)
        {
            // Arrange
            var expectedName = name;
            var sonar = new Sonar(expectedName);

            // Act
            var actualName = sonar.Name;

            // Assert
            Assert.Equal(expectedName, actualName);
        }
    }
}
