using FCG.Application.Shared.Models;
using FCG.Domain.Entities;
using FCG.Domain.Repositories.GamesRepository;
using Microsoft.EntityFrameworkCore;

namespace FCG.Application.UseCases.Games.GetAll
{
    public class GetAllGamesUseCase : IGetAllGamesUseCase
    {
        private readonly IReadOnlyGameRepository _readOnlyGameRepository;

        public GetAllGamesUseCase(IReadOnlyGameRepository readOnlyGameRepository)
        {
            _readOnlyGameRepository = readOnlyGameRepository;
        }

        public async Task<PagedListResponse<GetAllGamesOutput>> Handle(GetAllGamesInput request, CancellationToken cancellationToken)
        {
            var query = _readOnlyGameRepository.GetAllAsQueryable();

            query = ApplyFilters(query, request.Filter);

            var totalCount = await query.CountAsync(cancellationToken);

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
