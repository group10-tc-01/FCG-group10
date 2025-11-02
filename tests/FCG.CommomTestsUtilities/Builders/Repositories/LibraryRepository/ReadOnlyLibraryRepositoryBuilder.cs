using FCG.Domain.Entities;
using FCG.Domain.Repositories.LibraryRepository;
using Moq;

namespace FCG.CommomTestsUtilities.Builders.Repositories.LibraryRepository
{
    public class ReadOnlyLibraryRepositoryBuilder
    {
        private static readonly Mock<IReadOnlyLibraryRepository> _readOnlyLibraryRepoMock = new();

        public static IReadOnlyLibraryRepository Build()
        {
            return _readOnlyLibraryRepoMock.Object;
        }

        public static void SetupGetByUserIdAsync(Library? library)
        {
            _readOnlyLibraryRepoMock
                .Setup(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(library);
        }

        public static void SetupGetByUserIdWithGamesAsync(Library? library)
        {
            _readOnlyLibraryRepoMock
                .Setup(x => x.GetByUserIdWithGamesAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(library);
        }
    }
}
