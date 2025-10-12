using FCG.Application.Shared.Models;
using FCG.Application.UseCases.AdminUsers.GetAllUsers.GetAllUserDTO;
using FCG.Domain.Enum;
using FCG.Domain.Repositories.UserRepository;
using MediatR;

namespace FCG.Application.UseCases.AdminUsers.GetAllUsers
{
    public sealed class GetAllUsersUseCase : IRequestHandler<GetAllUserCaseQuery, PagedListResponse<UserListResponse>>
    {
        private readonly IReadOnlyUserRepository _userRepository;

        public GetAllUsersUseCase(IReadOnlyUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PagedListResponse<UserListResponse>> Handle(
            GetAllUserCaseQuery request,
            CancellationToken cancellationToken)
        {
            var (users, totalCount) = await _userRepository.GetQueryableAllUsers(
                request.Email,
                request.Role,
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            if (totalCount == 0)
            {
                return new PagedListResponse<UserListResponse>(
                    new List<UserListResponse>(),
                    0,
                    request.PageNumber,
                    request.PageSize
                );
            }
            var items = users.Select(u => new UserListResponse
            {
                Id = u.Id,
                Name = u.Name.Value,
                Email = u.Email.Value,
                CreatedAt = u.CreatedAt,
                Role = u.Role.ToString()
            }).ToList();

            return new PagedListResponse<UserListResponse>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }


    }


}