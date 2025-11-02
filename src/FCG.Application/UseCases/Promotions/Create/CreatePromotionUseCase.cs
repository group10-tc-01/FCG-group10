using FCG.Application.UseCases.Users.Update;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.GamesRepository;
using FCG.Domain.Repositories.PromotionRepository;
using FCG.Domain.Services;
using FCG.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Promotions.Create
{
    public class CreatePromotionUseCase : ICreatePromotionUseCase
    {
        private readonly IReadOnlyGameRepository _readOnlyGameRepository;
        private readonly IReadOnlyPromotionRepository _readOnlyPromotionRepository;
        private readonly IWriteOnlyPromotionRepository _writeOnlyPromotionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly ILogger<UpdateUserUseCase> _logger;

        public CreatePromotionUseCase(
            IReadOnlyGameRepository readOnlyGameRepository,
            IReadOnlyPromotionRepository readOnlyPromotionRepository,
            IWriteOnlyPromotionRepository writeOnlyPromotionRepository,
            IUnitOfWork unitOfWork,
            ICorrelationIdProvider correlationIdProvider,
            ILogger<UpdateUserUseCase> logger)
        {
            _readOnlyGameRepository = readOnlyGameRepository;
            _readOnlyPromotionRepository = readOnlyPromotionRepository;
            _writeOnlyPromotionRepository = writeOnlyPromotionRepository;
            _unitOfWork = unitOfWork;
            _correlationIdProvider = correlationIdProvider;
            _logger = logger;
        }

        public async Task<CreatePromotionResponse> Handle(CreatePromotionRequest request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationIdProvider.GetCorrelationId();

            _logger.LogInformation(
                "[CreatePromotionUseCase] [CorrelationId: {CorrelationId}] Creating promotion for game: {GameId}",
                correlationId, request.GameId);

            var gameExists = await _readOnlyGameRepository.ExistsAsync(request.GameId, cancellationToken);

            if (!gameExists)
            {
                throw new NotFoundException("Game not found.");
            }

            var hasActivePromotion = await _readOnlyPromotionRepository.ExistsActivePromotionForGameAsync(
                request.GameId,
                request.StartDate,
                request.EndDate,
                cancellationToken);

            if (hasActivePromotion)
            {
                throw new BadRequestException("An active promotion already exists for this game in the specified period.");
            }

            var discount = Discount.Create(request.DiscountPercentage);
            var promotion = Promotion.Create(request.GameId, discount, request.StartDate, request.EndDate);

            await _writeOnlyPromotionRepository.AddAsync(promotion, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "[CreatePromotionUseCase] [CorrelationId: {CorrelationId}] Successfully created promotion: {PromotionId} for game: {GameId}",
                correlationId, promotion.Id, promotion.GameId);

            return new CreatePromotionResponse
            {
                Id = promotion.Id,
                GameId = promotion.GameId,
                Discount = promotion.Discount.Value,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate
            };
        }
    }
}
