using FCG.Application.UseCases.Users.Update;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Users.Update.UsersDTO
{
    public class UpdateUserRequestTests
    {
        [Fact]
        public void Given_ValidProperties_When_CreateUpdateUserRequest_Then_ShouldSetAllPropertiesCorrectly()
        {
            // Arrange
            var currentPassword = "CurrentPass@123";
            var newPassword = "NewPass@456";

            // Act
            var request = new UpdateUserRequest
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            // Assert
            request.CurrentPassword.Should().Be(currentPassword);
            request.NewPassword.Should().Be(newPassword);
        }
    }
}