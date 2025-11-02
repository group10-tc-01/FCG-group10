using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.Admin.GetUserById
{
    [ExcludeFromCodeCoverage]
    public class LibraryDto
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; init; }
        public List<LibraryGameDto> Games { get; init; } = new();
    }
}
