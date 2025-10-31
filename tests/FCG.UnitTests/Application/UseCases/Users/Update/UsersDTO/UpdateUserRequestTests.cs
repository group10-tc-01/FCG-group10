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
            var bodyRequest = new UpdateUserBodyRequest
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            // Act
            var request = new UpdateUserRequest(bodyRequest);

            // Assert
            request.CurrentPassword.Should().Be(currentPassword);
            request.NewPassword.Should().Be(newPassword);
        }
    }
}