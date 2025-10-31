using FCG.Domain.Enum;
using FCG.Domain.Models.Pagination;
using MediatR;

namespace FCG.Application.UseCases.Games.GetAll
{
    public class GetAllGamesInput : PaginationParams, IRequest<PagedListResponse<GetAllGamesOutput>>
    {
        public string? Name { get; set; }
        public GameCategory? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
