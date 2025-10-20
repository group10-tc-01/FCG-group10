using FCG.Application.UseCases.Users.Update.UsersDTO;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Users.Update.UsersDTO
{
    public class UpdateUserRequestTests
    {
        [Fact(DisplayName = "Deve criar UpdateUserRequest com propriedades corretas")]
        public void CreateUpdateUserRequest_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var currentPassword = "CurrentPass@123";
            var newPassword = "NewPass@456";

            // Act
            var request = new UpdateUserRequest
            {
                Id = id,
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            // Assert
            request.Id.Should().Be(id);
            request.CurrentPassword.Should().Be(currentPassword);
            request.NewPassword.Should().Be(newPassword);
        }
    }
}