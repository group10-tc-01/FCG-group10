using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.LibraryGameRespository
{
    public interface IWriteOnlyLibraryGameRepository
    {
        Task CreateAsync(LibraryGame libraryGame);
    }
}
