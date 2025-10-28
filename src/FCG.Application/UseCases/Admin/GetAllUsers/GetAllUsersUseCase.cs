using FCG.Domain.Models.Pagination;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Admin.GetAllUsers
{
    public class GetAllUsersUseCase : IGetAllUsersUseCase
    {
        private readonly IReadOnlyUserRepository _userRepository;
        private readonly ILogger<GetAllUsersUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public GetAllUsersUseCase(
            IReadOnlyUserRepository userRepository,
            ILogger<GetAllUsersUseCase> logger,
            ICorrelationIdProvider correlationIdProvider)
        {
            _userRepository = userRepository;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task<PagedListResponse<GetAllUsersResponse>> Handle(GetAllUserCaseRequest request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            _logger.LogInformation(
                "[GetAllUsersUseCase] [CorrelationId: {CorrelationId}] Getting users - Page: {PageNumber}, Size: {PageSize}",
                correlationId, request.PageNumber, request.PageSize);

            var (users, totalCount) = await _userRepository.GetAllUsersAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            if (totalCount == 0)
            {
                _logger.LogInformation(
                    "[GetAllUsersUseCase] [CorrelationId: {CorrelationId}] No users found",
                    correlationId);

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

            _logger.LogInformation(
                "[GetAllUsersUseCase] [CorrelationId: {CorrelationId}] Successfully retrieved {Count} users out of {TotalCount}",
                correlationId, items.Count, totalCount);

            return new PagedListResponse<GetAllUsersResponse>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }
    }
}