using FCG.Application.Shared.Models;
using FCG.Application.UseCases.AdminUsers.GetAllUsers;
using FCG.Application.UseCases.AdminUsers.GetAllUsers.GetAllUserDTO;
using FCG.Application.UseCases.AdminUsers.GetById;
using FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO;
using FCG.Application.UseCases.Users.Register;
using FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.Application.UseCases.Users.Update;
using FCG.Application.UseCases.Users.Update.UsersDTO;
using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.Users
{
    [ExcludeFromCodeCoverage]
    public class UserManagementUseCase : IUserManagementUseCase
    {
        private readonly GetAllUsersUseCase _getAllUsersHandler;
        private readonly GetByIdUserUseCase _getByIdUserHandler;
        private readonly RegisterUserUseCase _registerUserHandler;
        private readonly UpdateUserUseCase _updateUserHandler;
        public UserManagementUseCase(
                GetAllUsersUseCase getAllUsersHandler,
                GetByIdUserUseCase getByIdUserHandler,
                RegisterUserUseCase registerUserHandler,
                UpdateUserUseCase updateUserHandler)
        {
            _getAllUsersHandler = getAllUsersHandler;
            _getByIdUserHandler = getByIdUserHandler;
            _registerUserHandler = registerUserHandler;
            _updateUserHandler = updateUserHandler;
        }

        public Task<PagedListResponse<UserListResponse>> GetUserAsync(GetAllUserCaseQuery query)
        {
            return _getAllUsersHandler.Handle(query, CancellationToken.None);
        }
        public Task<UserDetailResponse> GetUserByIdAsync(GetByIdUserQuery request)
        {
            return _getByIdUserHandler.Handle(request, CancellationToken.None);
        }
        public Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
        {
            return _registerUserHandler.Handle(request, CancellationToken.None);
        }
        public Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            return _updateUserHandler.Handle(request, CancellationToken.None);
        }


    }
}
