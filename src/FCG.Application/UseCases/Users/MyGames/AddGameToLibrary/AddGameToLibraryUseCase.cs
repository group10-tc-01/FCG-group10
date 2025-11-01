using System.Diagnostics.CodeAnalysis;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.GamesRepository;
using FCG.Domain.Repositories.LibraryGameRespository;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Services;
using MediatR;

namespace FCG.Application.UseCases.Users.MyGames.AddGameToLibrary
{
    [ExcludeFromCodeCoverage]
    public class AddGameToLibraryUseCase : IRequestHandler<AddGameToLibraryRequest, LibraryGameResponse>
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IReadOnlyLibraryRepository _libraryRepository;
        private readonly IReadOnlyGameRepository _gameRepository;
        private readonly IReadOnlyLibraryGameRepository _readLibraryGameRepo;
        private readonly IWriteOnlyLibraryGameRepository _writeLibraryGameRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AddGameToLibraryUseCase(
            ILoggedUser loggedUser,
            IReadOnlyLibraryRepository libraryRepository,
            IReadOnlyGameRepository gameRepository,
            IReadOnlyLibraryGameRepository readLibraryGameRepo,
            IWriteOnlyLibraryGameRepository writeLibraryGameRepo,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _libraryRepository = libraryRepository;
            _gameRepository = gameRepository;
            _readLibraryGameRepo = readLibraryGameRepo;
            _writeLibraryGameRepo = writeLibraryGameRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<LibraryGameResponse> Handle(AddGameToLibraryRequest request, CancellationToken cancellationToken)
        {

            var userId = await _loggedUser.GetLoggedUserAsync();


            var game = await _gameRepository.GetByIdAsync(request.GameId);
            if (game == null)
            {
                throw new NotFoundException("Jogo não encontrado.");
            }

            var library = await _libraryRepository.GetByUserIdAsync(userId.Id);
            if (library == null)
            {
                throw new NotFoundException("Biblioteca do usuário não encontrada.");
            }


            var existingGame = await _readLibraryGameRepo.GetByLibraryAndGameIdAsync(library.Id, game.Id);
            if (existingGame != null)
            {
                throw new ConflictException("Este jogo já está na sua biblioteca.");
            }

            var newLibraryGame = LibraryGame.Create(library.Id, game.Id, game.Price);

            await _writeLibraryGameRepo.CreateAsync(newLibraryGame);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LibraryGameResponse
            {
                GameId = newLibraryGame.GameId,
                GameName = game.Name.Value,
                PurchaseDate = newLibraryGame.PurchaseDate,
                PurchasePrice = newLibraryGame.PurchasePrice.Value,
                Status = newLibraryGame.Status
            };
        }
    }
}