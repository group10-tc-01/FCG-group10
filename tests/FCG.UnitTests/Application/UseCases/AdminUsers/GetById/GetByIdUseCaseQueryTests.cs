using FCG.Application.UseCases.AdminUsers.GetById;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.AdminUsers.GetById
{
    public class GetByIdUseCaseQueryTests
    {
        [Fact(DisplayName = "Construtor deve definir Id corretamente")]
        public void Constructor_GivenValidId_ShouldSetIdCorrectly()
        {
            // Arrange
            var expectedId = Guid.NewGuid();

            // Act
            var query = new GetByIdUserQuery(expectedId);

            // Assert
            query.Id.Should().Be(expectedId);
        }

        [Fact(DisplayName = "Deve criar query com Guid vazio")]
        public void Constructor_GivenEmptyGuid_ShouldCreateQuery()
        {
            // Arrange
            var emptyId = Guid.Empty;

            // Act
            var query = new GetByIdUserQuery(emptyId);

            // Assert
            query.Id.Should().Be(Guid.Empty);
        }
    }
}