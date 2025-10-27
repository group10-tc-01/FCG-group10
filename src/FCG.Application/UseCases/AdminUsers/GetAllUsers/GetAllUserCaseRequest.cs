using FCG.Domain.Models.Pagination;
using MediatR;

namespace FCG.Application.UseCases.AdminUsers.GetAllUsers
{
    public class GetAllUserCaseRequest : PaginationParams, IRequest<PagedListResponse<GetAllUsersResponse>> { }
}
