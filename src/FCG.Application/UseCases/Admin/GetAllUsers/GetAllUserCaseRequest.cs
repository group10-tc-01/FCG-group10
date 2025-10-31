using FCG.Domain.Models.Pagination;
using MediatR;

namespace FCG.Application.UseCases.Admin.GetAllUsers
{
    public class GetAllUserCaseRequest : PaginationParams, IRequest<PagedListResponse<GetAllUsersResponse>>
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
