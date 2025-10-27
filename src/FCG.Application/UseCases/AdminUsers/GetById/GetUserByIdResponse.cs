namespace FCG.Application.UseCases.AdminUsers.GetById
{
    public class GetUserByIdResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public LibraryDto? Library { get; init; }
        public WalletDto? Wallet { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
