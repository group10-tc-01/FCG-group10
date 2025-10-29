using FCG.Domain.Entities;
using FCG.Domain.Models.Pagination;
using FCG.Domain.Repositories.GamesRepository;
using FCG.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Games.GetAll
{
    public class GetAllGamesUseCase : IGetAllGamesUseCase
    {
        private readonly IReadOnlyGameRepository _readOnlyGameRepository;
        private readonly ILogger<GetAllGamesUseCase> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public GetAllGamesUseCase(IReadOnlyGameRepository readOnlyGameRepository, ILogger<GetAllGamesUseCase> logger, ICorrelationIdProvider correlationIdProvider)
        {
            _readOnlyGameRepository = readOnlyGameRepository;
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task<PagedListResponse<GetAllGamesOutput>> Handle(GetAllGamesInput request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            _logger.LogInformation(
                "[GetAllGamesHandler] [CorrelationId: {CorrelationId}] Starting GetAllGames request. Filters: Name={Name}, Category={Category}, MinPrice={MinPrice}, MaxPrice={MaxPrice}, PageNumber={PageNumber}, PageSize={PageSize}",
                correlationId, request.Filter.Name, request.Filter.Category, request.Filter.MinPrice, request.Filter.MaxPrice, request.PageNumber, request.PageSize);

            var query = _readOnlyGameRepository.GetAllAsQueryable();

            _logger.LogDebug(
                "[GetAllGamesHandler] [CorrelationId: {CorrelationId}] Applying filters to query",
                correlationId);

            query = ApplyFilters(query, request.Filter);

            _logger.LogDebug(
                "[GetAllGamesHandler] [CorrelationId: {CorrelationId}] Counting total items",
                correlationId);

            var totalCount = await query.CountAsync(cancellationToken);

            _logger.LogInformation(
                "[GetAllGamesHandler] [CorrelationId: {CorrelationId}] Total items found: {TotalCount}",
                correlationId, totalCount);

            var pagedQuery = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var items = await pagedQuery
                .Select(x => new GetAllGamesOutput
                {
                    Id = x.Id,
                    Category = x.Category,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation(
                "[GetAllGamesHandler] [CorrelationId: {CorrelationId}] Successfully retrieved {ItemCount} games out of {TotalCount} total. PageNumber={PageNumber}, PageSize={PageSize}",
                correlationId, items.Count, totalCount, request.PageNumber, request.PageSize);

            return new PagedListResponse<GetAllGamesOutput>(items, totalCount, request.PageNumber, request.PageSize);
        }

        private static IQueryable<Game> ApplyFilters(IQueryable<Game> query, GameFilter? filter)
        {
            if (filter is null)
                return query;

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(g => g.Name.Value.Contains(filter.Name));

            if (!string.IsNullOrWhiteSpace(filter.Category))
                query = query.Where(g => g.Category == filter.Category);

            if (filter.MinPrice.HasValue)
                query = query.Where(g => g.Price.Value >= filter.MinPrice.Value);

            if (filter.MaxPrice.HasValue)
                query = query.Where(g => g.Price.Value <= filter.MaxPrice.Value);

            return query;
        }
    }
}
