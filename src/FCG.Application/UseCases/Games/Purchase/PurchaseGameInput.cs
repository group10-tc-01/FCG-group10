using MediatR;

namespace FCG.Application.UseCases.Games.Purchase
{
    public record PurchaseGameInput(Guid Id) : IRequest<PurchaseGameOutput>
    {
    }
}
