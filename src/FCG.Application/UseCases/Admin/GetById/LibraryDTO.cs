using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.Admin.GetById
{
    [ExcludeFromCodeCoverage]
    public class LibraryDto
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
