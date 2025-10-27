using FCG.Application.UseCases.Users.Update;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Users.Update.UsersDTO
{
    public class UpdateUserResponseTests
    {
        [Fact(DisplayName = "Deve criar UpdateUserResponse com propriedades corretas")]
        public void CreateUpdateUserResponse_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updatedAt = DateTime.UtcNow;
            var message = "Usuário atualizado com sucesso!";

            // Act
            var response = new UpdateUserResponse
            {
                Id = id,
                UpdatedAt = updatedAt,
                Message = message
            };

            // Assert
            response.Id.Should().Be(id);
            response.UpdatedAt.Should().Be(updatedAt);
            response.Message.Should().Be(message);
        }
    }
}