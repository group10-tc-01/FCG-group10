using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.AdminUsers.GetAllUsers.GetAllUserDTO
{
    [ExcludeFromCodeCoverage]
    public class UserListResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
