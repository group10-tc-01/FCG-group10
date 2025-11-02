using FCG.Application.UseCases.Users.Register.UsersDTO.FCG.Application.UseCases.Users.Register.UsersDTO;
using MediatR;

namespace FCG.Application.UseCases.Users.Register
{
    public class RegisterUserRequest : IRequest<RegisterUserResponse>
    {
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
