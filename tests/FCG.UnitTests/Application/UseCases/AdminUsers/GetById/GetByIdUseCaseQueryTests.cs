using FCG.Application.UseCases.Admin.GetById;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Admin.GetById
{
    public class GetByIdUseCaseQueryTests
    {
        [Fact]
        public void Given_ValidId_When_Constructor_Then_ShouldSetIdCorrectly()
        {
            // Arrange
            var expectedId = Guid.NewGuid();

            // Act
            var query = new GetUserByIdRequest(expectedId);

            // Assert
            query.Id.Should().Be(expectedId);
        }

        [Fact]
        public void Given_EmptyGuid_When_Constructor_Then_ShouldCreateQuery()
        {
            // Arrange
            var emptyId = Guid.Empty;

            // Act
            var query = new GetUserByIdRequest(emptyId);

            // Assert
            query.Id.Should().Be(Guid.Empty);
        }
    }
}