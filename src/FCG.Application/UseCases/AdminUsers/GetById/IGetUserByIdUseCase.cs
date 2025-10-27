using MediatR;

namespace FCG.Application.UseCases.AdminUsers.GetById
{
    public interface IGetUserByIdUseCase : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse> { }
}