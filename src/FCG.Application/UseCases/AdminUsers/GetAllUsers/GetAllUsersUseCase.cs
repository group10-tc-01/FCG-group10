using FCG.Domain.Models.Pagination;
using FCG.Domain.Repositories.UserRepository;

namespace FCG.Application.UseCases.AdminUsers.GetAllUsers
{
    public class GetAllUsersUseCase : IGetAllUsersUseCase
    {
        private readonly IReadOnlyUserRepository _userRepository;

        public GetAllUsersUseCase(IReadOnlyUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PagedListResponse<GetAllUsersResponse>> Handle(GetAllUserCaseRequest request, CancellationToken cancellationToken)
        {
            var (users, totalCount) = await _userRepository.GetAllUsersAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            if (totalCount == 0)
            {
                return new PagedListResponse<GetAllUsersResponse>(
                    new List<GetAllUsersResponse>(),
                    0,
                    request.PageNumber,
                    request.PageSize
                );
            }

            var items = users.Select(u => new GetAllUsersResponse
            {
                Id = u.Id,
                Name = u.Name.Value,
                Email = u.Email.Value,
                CreatedAt = u.CreatedAt,
                Role = u.Role.ToString()
            }).ToList();

            return new PagedListResponse<GetAllUsersResponse>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }
    }
}