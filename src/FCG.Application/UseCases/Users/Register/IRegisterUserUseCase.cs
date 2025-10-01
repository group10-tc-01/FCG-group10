using FCG.Application.UseCases.UsersDTO;
using MediatR;

namespace FCG.Application.UseCases.Users.Register
{
    public interface IRegisterUserUseCase : IRequestHandler<RegisterUserRequest, RegisterUserResponse> { }
}