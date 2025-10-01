using FCG.Application.UseCases.Users.Register.UsersDTO;
using MediatR;

namespace FCG.Application.UseCases.Users.Register
{
    public interface IRegisterUserUseCase : IRequestHandler<RegisterUserRequest, RegisterUserResponse> { }
}