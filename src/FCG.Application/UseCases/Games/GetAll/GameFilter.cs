namespace FCG.Application.UseCases.Games.GetAll
{
    public class GameFilter
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
