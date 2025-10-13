using FCG.Domain.Entities;

namespace FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO
{
    public class UserDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public LibraryDTO? Library { get; set; }
        public WalletDTO? Wallet { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
