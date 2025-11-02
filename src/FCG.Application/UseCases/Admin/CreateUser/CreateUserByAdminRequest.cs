using FCG.Domain.Enum;
using MediatR;

namespace FCG.Application.UseCases.Admin.CreateUser
{
    public class CreateUserByAdminRequest : IRequest<CreateUserByAdminResponse>
    {
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public Role Role { get; init; }
    }
}
