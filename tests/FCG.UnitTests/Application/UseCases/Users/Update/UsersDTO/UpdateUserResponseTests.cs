using FCG.Application.UseCases.Users.Update;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Users.Update.UsersDTO
{
    public class UpdateUserResponseTests
    {
        [Fact]
        public void Given_ValidProperties_When_CreateUpdateUserResponse_Then_ShouldSetAllPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updatedAt = DateTime.UtcNow;

            // Act
            var response = new UpdateUserResponse
            {
                Id = id,
                UpdatedAt = updatedAt
            };

            // Assert
            response.Id.Should().Be(id);
            response.UpdatedAt.Should().Be(updatedAt);
        }
    }
}