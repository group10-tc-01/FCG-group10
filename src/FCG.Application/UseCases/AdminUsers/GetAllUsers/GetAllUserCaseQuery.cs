using FCG.Application.Shared.Models;
using FCG.Application.Shared.Params;
using FCG.Application.UseCases.AdminUsers.GetAllUsers.GetAllUserDTO;
using MediatR;

namespace FCG.Application.UseCases.AdminUsers.GetAllUsers
{
    public class GetAllUserCaseQuery : PaginationParams, IRequest<PagedListResponse<UserListResponse>>
    {
        public string? Email { get; set; }
    }
}
