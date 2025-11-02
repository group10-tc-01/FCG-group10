using MediatR;

namespace FCG.Application.UseCases.Promotions.Create
{
    public interface ICreatePromotionUseCase : IRequestHandler<CreatePromotionRequest, CreatePromotionResponse> { }
}
