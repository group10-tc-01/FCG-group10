using FCG.Application.UseCases.UsersDTO;
using MediatR;

namespace FCG.Application.UseCases.Users.RegisterUser
{
    public interface IRegisterUserUseCase : IRequestHandler<RegisterUserRequest, RegisterUserResponse> { }
}