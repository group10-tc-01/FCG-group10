using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.Admin.GetUserById
{
    [ExcludeFromCodeCoverage]
    public class LibraryGameDto
    {
        public Guid GameId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public decimal PurchasePrice { get; init; }
        public DateTime PurchaseDate { get; init; }
    }
}
