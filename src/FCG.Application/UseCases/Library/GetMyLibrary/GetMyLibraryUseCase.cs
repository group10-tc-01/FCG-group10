using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Library.GetMyLibrary
{
    public sealed class GetMyLibraryUseCase : IGetMyLibraryUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IReadOnlyLibraryRepository _readOnlyLibraryRepository;
        private readonly ILogger<GetMyLibraryUseCase> _logger;

        public GetMyLibraryUseCase(
            ILoggedUser loggedUser,
            IReadOnlyLibraryRepository readOnlyLibraryRepository,
            ILogger<GetMyLibraryUseCase> logger)
        {
            _loggedUser = loggedUser;
            _readOnlyLibraryRepository = readOnlyLibraryRepository;
            _logger = logger;
        }

        public async Task<GetMyLibraryResponse> Handle(GetMyLibraryRequest request, CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();
            _logger.LogInformation("[{CorrelationId}] Getting library for logged user", correlationId);

            var loggedUser = await _loggedUser.GetLoggedUserAsync();

            _logger.LogInformation("[{CorrelationId}] Retrieving library for user {UserId}", correlationId, loggedUser.Id);

            var library = await _readOnlyLibraryRepository.GetByUserIdWithGamesAsync(loggedUser.Id, cancellationToken);

            if (library is null)
            {
                _logger.LogWarning("[{CorrelationId}] Library not found for user {UserId}", correlationId, loggedUser.Id);
                throw new NotFoundException("Library not found.");
            }

            var response = new GetMyLibraryResponse
            {
                LibraryId = library.Id,
                UserId = library.UserId,
                Games = library.LibraryGames.Select(lg => new MyLibraryGameDto
                {
                    GameId = lg.GameId,
                    Name = lg.Game!.Name.Value,
                    Description = lg.Game.Description,
                    CurrentPrice = lg.Game.Price.Value,
                    PurchasePrice = lg.PurchasePrice.Value,
                    PurchaseDate = lg.PurchaseDate,
                    Category = lg.Game.Category.ToString()
                }).ToList()
            };

            _logger.LogInformation("[{CorrelationId}] Successfully retrieved library with {GamesCount} games for user {UserId}",
                correlationId, response.Games.Count, loggedUser.Id);

            return response;
        }
    }
}
