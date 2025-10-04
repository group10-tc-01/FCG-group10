using FCG.Application.Shared.Models;
using FCG.Application.Shared.Params;
using MediatR;

namespace FCG.Application.UseCases.Users.GetAllUsers
{
    public class GetAllUserCaseQuery : PaginationParams, IRequest<PagedListResponse<UserListResponse>>
    {
    }
}
