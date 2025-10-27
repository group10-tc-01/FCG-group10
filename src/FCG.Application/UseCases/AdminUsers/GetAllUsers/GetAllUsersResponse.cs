using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.AdminUsers.GetAllUsers
{
    [ExcludeFromCodeCoverage]
    public class GetAllUsersResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
