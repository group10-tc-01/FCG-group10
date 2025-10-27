using MediatR;

namespace FCG.Application.UseCases.Admin.GetById
{
    public interface IGetUserByIdUseCase : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse> { }
}