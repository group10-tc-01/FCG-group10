using FCG.Domain.Models.Pagination;
using MediatR;

namespace FCG.Application.UseCases.Games.GetAll
{
    public interface IGetAllGamesUseCase : IRequestHandler<GetAllGamesInput, PagedListResponse<GetAllGamesOutput>>;
}
