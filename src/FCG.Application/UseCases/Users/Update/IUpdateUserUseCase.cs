using FCG.Application.UseCases.Users.Update.UsersDTO;
using MediatR;

namespace FCG.Application.UseCases.Users.Update
{
    public interface IUpdateUserUseCase : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
    {
    }
}
