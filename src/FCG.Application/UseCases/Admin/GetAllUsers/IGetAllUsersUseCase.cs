using FCG.Domain.Models.Pagination;
using MediatR;

namespace FCG.Application.UseCases.Admin.GetAllUsers
{
    public interface IGetAllUsersUseCase : IRequestHandler<GetAllUserCaseRequest, PagedListResponse<GetAllUsersResponse>> { }
}