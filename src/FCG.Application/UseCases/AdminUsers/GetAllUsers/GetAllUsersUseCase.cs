using FCG.Application.Shared.Models;
using FCG.Application.UseCases.AdminUsers.GetAllUsers.GetAllUserDTO;
using FCG.Domain.Entities;
using FCG.Domain.Repositories.UserRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var baseQuery = await _userRepository.GetQueryableAllUsers();

            var totalCountBeforeFilter = await baseQuery.CountAsync(cancellationToken);
            IEnumerable<User> filteredUsers;

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var searchEmail = request.Email.Trim().ToLower();

                var allUsers = await baseQuery
                    .OrderBy(u => u.Id)
                    .ToListAsync(cancellationToken);

                filteredUsers = allUsers
                    .Where(u => u.Email.Value.ToLower().Contains(searchEmail));
            }
            else
            {
                filteredUsers = await baseQuery
                    .OrderBy(u => u.Id)
                    .ToListAsync(cancellationToken);
            }
            var totalCount = filteredUsers.Count();

            if (totalCount == 0)
            {
                return new PagedListResponse<UserListResponse>(
                    new List<UserListResponse>(),
                    0,
                    request.PageNumber,
                    request.PageSize
                );
            }

            var pagedEntities = filteredUsers
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var items = pagedEntities.Select(u => new UserListResponse
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