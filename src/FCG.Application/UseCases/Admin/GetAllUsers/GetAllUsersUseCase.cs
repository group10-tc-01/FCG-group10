using FCG.Domain.Models.Pagination;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using Microsoft.EntityFrameworkCore;
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
                "[GetAllUsersUseCase] [CorrelationId: {CorrelationId}] Getting users - Filters: Name={Name}, Email={Email}, Page: {PageNumber}, Size: {PageSize}",
                correlationId, request.Name, request.Email, request.PageNumber, request.PageSize);

            _logger.LogDebug(
                "[GetAllUsersUseCase] [CorrelationId: {CorrelationId}] Applying filters to query",
                correlationId);

            var query = _userRepository.GetAllUsersWithFilters(
                name: request.Name,
                email: request.Email);

            _logger.LogDebug(
                "[GetAllUsersUseCase] [CorrelationId: {CorrelationId}] Counting total items",
                correlationId);

            var totalCount = await query.CountAsync(cancellationToken);

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

            var pagedQuery = query
                .OrderBy(u => u.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var items = await pagedQuery
                .Select(u => new GetAllUsersResponse
                {
                    Id = u.Id,
                    Name = u.Name.Value,
                    Email = u.Email.Value,
                    CreatedAt = u.CreatedAt,
                    Role = u.Role.ToString()
                })
                .ToListAsync(cancellationToken);

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