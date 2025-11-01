using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.LibraryGameRespository
{
    public interface IReadOnlyLibraryGameRepository
    {
        Task<IEnumerable<LibraryGame>> GetLibraryGamesByUserIdAsync(Guid userId);
        Task<LibraryGame?> GetByLibraryAndGameIdAsync(Guid libraryId, Guid gameId);
    }
}
