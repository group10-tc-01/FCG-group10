namespace FCG.Application.UseCases.Games.Purchase
{
    public record PurchaseGameOutput(string GameName, decimal OriginalPrice, decimal FinalPrice);
}
