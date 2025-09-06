using FCG.Domain;

namespace FCG.UnitTests
{
    public class SonarTest
    {
        [Fact]
        public void Sonar_Should_Have_Correct_Name()
        {
            // Arrange
            var expectedName = "TestSonar";
            var sonar = new Sonar(expectedName);

            // Act
            var actualName = sonar.Name;

            // Assert
            Assert.Equal(expectedName, actualName);
        }
    }
}
