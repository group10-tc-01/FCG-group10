using MediatR;

namespace FCG.Application.UseCases.UsersDTO
{
    public class RegisterUserRequest : IRequest<RegisterUserResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
