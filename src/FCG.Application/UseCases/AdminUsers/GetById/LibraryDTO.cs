using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.AdminUsers.GetById
{
    [ExcludeFromCodeCoverage]
    public class LibraryDto
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
