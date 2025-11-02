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
                correlationId, request.Name, request.Category, request.MinPrice, request.MaxPrice, request.PageNumber, request.PageSize);

            _logger.LogDebug(
                "[GetAllGamesHandler] [CorrelationId: {CorrelationId}] Applying filters to query",
                correlationId);

            var query = _readOnlyGameRepository.GetAllWithFilters(
                name: request.Name,
                category: request.Category,
                minPrice: request.MinPrice,
                maxPrice: request.MaxPrice);

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

            var games = await pagedQuery.ToListAsync(cancellationToken);

            var items = games.Select(x =>
            {
                var now = DateTime.UtcNow;
                var activePromotion = x!.Promotions?
                    .Where(p => p.StartDate <= now && p.EndDate >= now && p.IsActive)
                    .OrderByDescending(p => p.Discount.Value)
                    .FirstOrDefault();

                var originalPrice = x.Price.Value;
                var finalPrice = originalPrice;

                ActivePromotionDto? promotionDto = null;

                if (activePromotion != null)
                {
                    var discountAmount = originalPrice * (activePromotion.Discount.Value / 100);
                    finalPrice = originalPrice - discountAmount;

                    promotionDto = new ActivePromotionDto
                    {
                        PromotionId = activePromotion.Id,
                        DiscountPercentage = activePromotion.Discount.Value,
                        StartDate = activePromotion.StartDate,
                        EndDate = activePromotion.EndDate
                    };
                }

                return new GetAllGamesOutput
                {
                    Id = x.Id,
                    Category = x.Category.ToString(),
                    Description = x.Description,
                    Name = x.Name.Value,
                    Price = originalPrice,
                    FinalPrice = finalPrice,
                    ActivePromotion = promotionDto
                };
            }).ToList();

            _logger.LogInformation(
                "[GetAllGamesHandler] [CorrelationId: {CorrelationId}] Successfully retrieved {ItemCount} games out of {TotalCount} total. Games with active promotions: {PromotionCount}. PageNumber={PageNumber}, PageSize={PageSize}",
                correlationId, items.Count, totalCount, items.Count(x => x.ActivePromotion != null), request.PageNumber, request.PageSize);

            return new PagedListResponse<GetAllGamesOutput>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
