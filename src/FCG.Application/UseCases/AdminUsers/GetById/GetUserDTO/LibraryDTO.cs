using System.Diagnostics.CodeAnalysis;

namespace FCG.Application.UseCases.AdminUsers.GetById.GetUserDTO
{
    [ExcludeFromCodeCoverage]
    public class LibraryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
