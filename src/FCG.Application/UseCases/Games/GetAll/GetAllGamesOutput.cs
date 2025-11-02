namespace FCG.Application.UseCases.Games.GetAll
{
    public class GetAllGamesOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public ActivePromotionDto? ActivePromotion { get; set; }
        public decimal FinalPrice { get; set; }
    }

    public class ActivePromotionDto
    {
        public Guid PromotionId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
