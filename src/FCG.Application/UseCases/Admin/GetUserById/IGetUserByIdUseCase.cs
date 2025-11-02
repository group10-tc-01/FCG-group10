using MediatR;

namespace FCG.Application.UseCases.Admin.GetUserById
{
    public interface IGetUserByIdUseCase : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse> { }
}