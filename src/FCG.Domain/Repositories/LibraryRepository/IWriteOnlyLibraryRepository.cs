using FCG.Domain.Entities;

namespace FCG.Domain.Repositories.LibraryRepository
{
    public interface IWriteOnlyLibraryRepository
    {
        Task AddAsync(Library library);
    }
}
