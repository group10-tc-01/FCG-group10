namespace FCG.Application.UseCases.Library.GetMyLibrary
{
    public class GetMyLibraryResponse
    {
        public Guid LibraryId { get; set; }
        public Guid UserId { get; set; }
        public List<LibraryGameDto> Games { get; set; } = new();
    }

    public class LibraryGameDto
    {
        public Guid GameId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal CurrentPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
