using FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.AdminUsers.GetById.GetUserDTO
{
    public class UserDetailResponseTests
    {
        [Fact(DisplayName = "Deve criar UserDetailResponse com propriedades corretas")]
        public void CreateUserDetailResponse_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "Test User";
            var email = "test@example.com";
            var role = "User";
            var createdAt = DateTime.UtcNow;
            var wallet = new WalletDto { Id = Guid.NewGuid(), Balance = 100 };
            var library = new LibraryDto { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow };

            // Act
            var response = new UserDetailResponse
            {
                Id = id,
                Name = name,
                Email = email,
                Role = role,
                CreatedAt = createdAt,
                Wallet = wallet,
                Library = library
            };

            // Assert
            response.Id.Should().Be(id);
            response.Name.Should().Be(name);
            response.Email.Should().Be(email);
            response.Role.Should().Be(role);
            response.CreatedAt.Should().Be(createdAt);
            response.Wallet.Should().Be(wallet);
            response.Library.Should().Be(library);
        }
    }
}