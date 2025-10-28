using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.GamesRepository;
using FCG.Domain.Repositories.PromotionRepository;
using FCG.Domain.ValueObjects;

namespace FCG.Application.UseCases.Promotions.Create
{
    public class CreatePromotionUseCase : ICreatePromotionUseCase
    {
        private readonly IReadOnlyGameRepository _readOnlyGameRepository;
        private readonly IReadOnlyPromotionRepository _readOnlyPromotionRepository;
        private readonly IWriteOnlyPromotionRepository _writeOnlyPromotionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePromotionUseCase(
            IReadOnlyGameRepository readOnlyGameRepository,
            IReadOnlyPromotionRepository readOnlyPromotionRepository,
            IWriteOnlyPromotionRepository writeOnlyPromotionRepository,
            IUnitOfWork unitOfWork)
        {
            _readOnlyGameRepository = readOnlyGameRepository;
            _readOnlyPromotionRepository = readOnlyPromotionRepository;
            _writeOnlyPromotionRepository = writeOnlyPromotionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreatePromotionResponse> Handle(CreatePromotionRequest request, CancellationToken cancellationToken)
        {
            var gameExists = await _readOnlyGameRepository.ExistsAsync(request.GameId, cancellationToken);
            if (!gameExists)
            {
                throw new DomainException("Game not found.");
            }

            var hasActivePromotion = await _readOnlyPromotionRepository.ExistsActivePromotionForGameAsync(
                request.GameId,
                request.StartDate,
                request.EndDate,
                cancellationToken);

            if (hasActivePromotion)
            {
                throw new DomainException("An active promotion already exists for this game in the specified period.");
            }

            var discount = Discount.Create(request.DiscountPercentage);
            var promotion = Promotion.Create(request.GameId, discount, request.StartDate, request.EndDate);

            await _writeOnlyPromotionRepository.AddAsync(promotion, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

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
