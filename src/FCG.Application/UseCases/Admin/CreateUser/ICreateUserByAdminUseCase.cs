using MediatR;

namespace FCG.Application.UseCases.Admin.CreateUser
{
    public interface ICreateUserByAdminUseCase : IRequestHandler<CreateUserByAdminRequest, CreateUserByAdminResponse>
    {
    }
}
