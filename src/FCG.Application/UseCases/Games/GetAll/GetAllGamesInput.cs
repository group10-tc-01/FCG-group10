using FCG.Domain.Models.Pagination;
using MediatR;

namespace FCG.Application.UseCases.Games.GetAll
{
    public class GetAllGamesInput : PaginationParams, IRequest<PagedListResponse<GetAllGamesOutput>>
    {
        public GameFilter Filter { get; set; } = new();
    }
}
