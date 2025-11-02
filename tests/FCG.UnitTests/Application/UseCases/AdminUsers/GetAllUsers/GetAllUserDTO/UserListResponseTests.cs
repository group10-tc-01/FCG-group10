using FCG.Application.UseCases.Admin.GetAllUsers;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.AdminUsers.GetAllUsers.GetAllUserDTO
{
    public class UserListResponseTests
    {
        [Fact]
        public void Given_ValidProperties_When_CreateUserListResponse_Then_ShouldSetAllPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Test User";
            var email = "test@example.com";
            var role = "User";
            var createdAt = DateTime.UtcNow;

            // Act
            var response = new GetAllUsersResponse
            {
                Id = id,
                Name = name,
                Email = email,
                Role = role,
                CreatedAt = createdAt
            };

            // Assert
            response.Id.Should().Be(id);
            response.Name.Should().Be(name);
            response.Email.Should().Be(email);
            response.Role.Should().Be(role);
            response.CreatedAt.Should().Be(createdAt);
        }
    }
}