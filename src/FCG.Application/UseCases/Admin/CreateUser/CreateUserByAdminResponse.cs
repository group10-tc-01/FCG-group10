using FCG.Domain.Enum;

namespace FCG.Application.UseCases.Admin.CreateUser
{
    public class CreateUserByAdminResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public Role Role { get; init; }
    }
}
