using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.LibraryGameRespository;
using FCG.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.UseCases.Users.MyGames
{
    public class LibraryGameUseCase : ILibraryGameUseCase
    {
        private readonly IReadOnlyLibraryGameRepository _libraryGameRepository;
        private readonly ILoggedUser _loggedUser;

        public LibraryGameUseCase(IReadOnlyLibraryGameRepository libraryGameRepository, ILoggedUser loggedUser)
        {
            _libraryGameRepository = libraryGameRepository;
            _loggedUser = loggedUser;
        }
        public async Task<ICollection<LibraryGameResponse>> Handle(LibraryGameUseCaseRequest request, CancellationToken cancellationToken)
        {
            var userId = await _loggedUser.GetLoggedUserAsync();

            var libraryGames = await _libraryGameRepository.GetLibraryGamesByUserIdAsync(userId.Id);

            if (libraryGames == null || !libraryGames.Any())
            {
                throw new NotFoundException("Not game in library");
            }

            var responseList = libraryGames
                .Select(lg => new LibraryGameResponse
                {
                    GameId = lg.GameId,
                    GameName = lg.Game.Name.Value,
                    PurchaseDate = lg.PurchaseDate,
                    PurchasePrice = lg.PurchasePrice.Value,
                    Status = lg.Status
                })
                .ToList();

            return responseList;
        }
    }
}
