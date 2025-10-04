using MediatR;

namespace FCG.Application.UseCases.Users.GetAllUsers
{
    public class GetAllUserCaseQuery : IRequest<List<UserListResponse>>
    {
    }
}
