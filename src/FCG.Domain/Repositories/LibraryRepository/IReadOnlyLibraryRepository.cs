using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.LibraryRepository
{
    public interface IReadOnlyLibraryRepository
    {
        Task<Library?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Library?> GetByUserIdWithGamesAsync(Guid userId, CancellationToken cancellationToken);
    }
}
