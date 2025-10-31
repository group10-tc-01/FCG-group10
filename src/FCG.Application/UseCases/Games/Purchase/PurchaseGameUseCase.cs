using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.GamesRepository;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Repositories.PromotionRepository;
using FCG.Domain.Repositories.WalletRepository;
using FCG.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Games.Purchase
{
    public class PurchaseGameUseCase : IPurchaseGameUseCase
    {
        private readonly IReadOnlyGameRepository _readOnlyGameRepository;
        private readonly IReadOnlyWalletRepository _readOnlyWalletRepository;
        private readonly IReadOnlyLibraryRepository _readOnlyLibraryRepository;
        private readonly IReadOnlyPromotionRepository _readOnlyPromotionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PurchaseGameUseCase> _logger;
        private readonly ILoggedUser _loggedUser;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public PurchaseGameUseCase(
            IReadOnlyGameRepository readOnlyGameRepository,
            IReadOnlyWalletRepository readOnlyWalletRepository,
            IReadOnlyLibraryRepository readOnlyLibraryRepository,
            IReadOnlyPromotionRepository readOnlyPromotionRepository,
            IUnitOfWork unitOfWork,
            ILogger<PurchaseGameUseCase> logger,
            ILoggedUser loggedUser,
            ICorrelationIdProvider correlationIdProvider)
        {
            _readOnlyGameRepository = readOnlyGameRepository;
            _readOnlyWalletRepository = readOnlyWalletRepository;
            _readOnlyLibraryRepository = readOnlyLibraryRepository;
            _readOnlyPromotionRepository = readOnlyPromotionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _loggedUser = loggedUser;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task<PurchaseGameOutput> Handle(PurchaseGameInput request, CancellationToken cancellationToken)
        {
            try
            {
                var game = await _readOnlyGameRepository.GetByIdAsync(request.Id, cancellationToken);

                if (game == null)
                {
                    throw new NotFoundException($"Game with Id: {request.Id} was not found.");
                }

                var user = await _loggedUser.GetLoggedUserAsync();

                var library = await _readOnlyLibraryRepository.GetByUserIdAsync(user.Id, cancellationToken);

                var gameAlreadyOwned = library?.LibraryGames?.Any(lg => lg.GameId == game.Id) ?? false;

                if (gameAlreadyOwned)
                {
                    throw new BadRequestException($"User already owns the game: {game.Name}");
                }

                var promotions = await _readOnlyPromotionRepository.GetByGameIdAsync(game.Id, cancellationToken);

                //TODO: Ajustar para trazer do repository apenas a promoção ativa
                var activePromotion = promotions?.FirstOrDefault(p => p.StartDate <= DateTime.UtcNow && p.EndDate >= DateTime.UtcNow);

                var finalPrice = game.Price;

                if (activePromotion != null)
                {
                    var discountAmount = (game.Price * activePromotion.Discount.Value) / 100;
                    finalPrice = game.Price - discountAmount;

                    _logger.LogInformation(
                        "[PurchaseGameUseCase] Active promotion found. GameId: {GameId}, OriginalPrice: {OriginalPrice}, Discount: {Discount}%, FinalPrice: {FinalPrice}, CorrelationId: {CorrelationId}",
                        game.Id,
                        game.Price,
                        activePromotion.Discount.Value,
                        finalPrice,
                        _correlationIdProvider.GetCorrelationId());
                }

                var wallet = await _readOnlyWalletRepository.GetByUserIdAsync(user.Id, cancellationToken);
                wallet?.Debit(finalPrice);
                library!.AddGame(game.Id, finalPrice);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "[PurchaseGameUseCase] Game purchased successfully. UserId: {UserId}, GameId: {GameId}, GameName: {GameName}, OriginalPrice: {OriginalPrice}, FinalPrice: {FinalPrice}, PromotionApplied: {PromotionApplied}, CorrelationId: {CorrelationId}",
                    user.Id,
                    game.Id,
                    game.Name,
                    game.Price,
                    finalPrice,
                    activePromotion != null,
                    _correlationIdProvider.GetCorrelationId());

                return new PurchaseGameOutput
                {

                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                _logger.LogError(ex,
                    "[PurchaseGameUseCase] Error purchasing game. CorrelationId: {CorrelationId}",
                    _correlationIdProvider.GetCorrelationId());
                throw;
            }
        }
    }
}
