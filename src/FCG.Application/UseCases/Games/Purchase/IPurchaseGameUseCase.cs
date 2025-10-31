using MediatR;

namespace FCG.Application.UseCases.Games.Purchase
{
    public interface IPurchaseGameUseCase : IRequestHandler<PurchaseGameInput, PurchaseGameOutput> { }
}
