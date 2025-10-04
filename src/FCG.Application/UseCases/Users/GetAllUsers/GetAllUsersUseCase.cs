using FCG.Application.Shared.Models;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FCG.Application.UseCases.Users.GetAllUsers.GetAllUserDTO;

namespace FCG.Application.UseCases.Users.GetAllUsers
{
    public sealed class GetAllUsersUseCase : IRequestHandler<GetAllUserCaseQuery, PagedListResponse<UserListResponse>>
    {
        private readonly IReadOnlyUserRepository _userRepository;

        public GetAllUsersUseCase(IReadOnlyUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PagedListResponse<UserListResponse>> Handle(GetAllUserCaseQuery request,
            CancellationToken cancellationToken)
        {
            var baseQuery = await _userRepository.GetQueryableAllUsers();

            var totalCount = await baseQuery.CountAsync(cancellationToken);

            if (totalCount == 0)
            {
                throw new NotFoundException("Nenhum usuário foi encontrado na base de dados.");
            }

            var pagedEntities = await baseQuery
                .OrderBy(u => u.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var items = pagedEntities
                .Select(entity => new UserListResponse
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Email = entity.Email,
                    CreatedAt = entity.CreatedAt,
                    Role = entity.Role.ToString()
                })
                .ToList();
            return new PagedListResponse<UserListResponse>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }
    }
}