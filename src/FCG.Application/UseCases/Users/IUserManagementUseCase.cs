using FCG.Application.Shared.Models;
using FCG.Application.UseCases.AdminUsers.GetAllUsers;
using FCG.Application.UseCases.AdminUsers.GetAllUsers.GetAllUserDTO;
using FCG.Application.UseCases.AdminUsers.GetById;
using FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO;
using FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.Application.UseCases.Users.Update.UsersDTO;

namespace FCG.Application.UseCases.Users
{
    public interface IUserManagementUseCase
    {
        Task<PagedListResponse<UserListResponse>> GetUserAsync(GetAllUserCaseQuery query);
        Task<UserDetailResponse> GetUserByIdAsync(GetByIdUserQuery request);
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);
        Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);
    }
}
